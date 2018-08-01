(function (chessEngine, $, undefined) {
  var
    settings,
    currentPlayer = 'b',
    boardConfiguration = {
      'a1': 'wr', 'b1': 'wn', 'c1': 'wb', 'd1': 'wq', 'e1': 'wk', 'f1': 'wb', 'g1': 'wn', 'h1': 'wr',
      'a2': 'wp', 'b2': 'wp', 'c2': 'wp', 'd2': 'wp', 'e2': 'wp', 'f2': 'wp', 'g2': 'wp', 'h2': 'wp',
      'a7': 'bp', 'b7': 'bp', 'c7': 'bp', 'd7': 'bp', 'e7': 'bp', 'f7': 'bp', 'g7': 'bp', 'h7': 'bp',
      'a8': 'br', 'b8': 'bn', 'c8': 'bb', 'd8': 'bq', 'e8': 'bk', 'f8': 'bb', 'g8': 'bn', 'h8': 'br'
    },
    resolveCellName = function (htmlElement) {
      var numericId = parseInt($(htmlElement).parent().attr('id')),
          num = Math.floor(numericId / 8) + 1,
          letter = numericId % 8;
      return String.fromCharCode(97 + letter) + num;
    },
    resolveCellId = function (cellName) {
      return (cellName.charCodeAt(0) - 97) + 8 * (cellName.charCodeAt(1) - 49);
    },
    resolveImageUri = function (img) {
      return '../Images/chesspieces/' + img + '.png';
    },
    buildHtmlTable = function () {
      var html = '<h5 id="turn"></h5><h6 id="statusMessage"></h6><table id="chessTable">',
          whiteTile = false,
          renderFor = settings.renderFor,
          elements = [];

      for (var i = 8; i >= -1; --i) {
        for (var j = -1; j <= 8; ++j) {
          var imgName = 'blank',
              cellName = String.fromCharCode(97 + j) + (i + 1),
              cssClass = whiteTile ? "white" : "black",
              isBorder = i === -1 || i === 8 || j === -1 || j === 8,
              isCorner = (i === -1 || i === 8) && (j === -1 || j === 8);

          if (isBorder) {
            cssClass = 'border';
            if (isCorner) {
              cellName = '';
            } else if (i === -1 || i === 8) {
              cellName = String.fromCharCode(97 + j);
              cssClass += ' horizontalBorder';
            } else {
              cellName = i + 1;
              cssClass += ' verticalBorder';
            }

            elements.push('<td class="' + cssClass + '">' + cellName + '</td>');
          }
          else {
            if (boardConfiguration[cellName] !== undefined) {
              imgName = boardConfiguration[cellName];
            }

            elements.push('<td class="' + cssClass + '" id="' + (i * 8 + j) + '"><img src="' + resolveImageUri(imgName) + '"><br />' + (settings.inDebugMode ? cellName + ' ' + (i * 8 + j) : '') + '</td>');

            whiteTile = !whiteTile;
          }
        }
        whiteTile = !whiteTile;
      }
      if (renderFor === 'black') {
        elements.reverse();
      }
      for (i = 0; i < elements.length;) {
        if (i % 10 === 0) { html += '<tr>'; }
        html += elements[i];
        ++i;
        if (i % 10 === 0) { html += '</tr>'; }
      }
      html += '</table><div id="capturedPieces"></div>';
      return html;
    },
    fromImg,
    legalMoves = [],
    resolveLegalMoves = function (lastMove) {
      $.ajax({
        url: 'Home/ListLegalMoves/',
        data: 'GameId=' + settings.gameId + '&PlayerId=' + settings.playerId + '&lastMove=' + lastMove,
      }).done(function (data) {
        if (data.StatusMessage !== null && data.StatusMessage.length > 0) {
          $('#turn').html($('#turn').html() + ' ' + data.StatusMessage);
        }

        legalMoves = [];

        $.each(data.Moves, function () {
          legalMoves.push(this.toString());
        });
      });
    },
    setup = function (gameSettings) {
      settings = gameSettings;
      $(settings.board).html(buildHtmlTable());
      $(settings.board).find('img').draggable({
        revert: true,
        drag: function () {
          fromImg = this;
        }
      });
      $(settings.board).find('img').droppable({
        drop: function () {
          var toCellName = resolveCellName(this),
              fromCellName = resolveCellName(fromImg);
          if ($.inArray(fromCellName + '-' + toCellName, legalMoves) === -1) {
            return;
          }
          if (boardConfiguration[toCellName] !== undefined) {
            if ($('#capturedPieces img').length % 11 === 0) {
              $('#capturedPieces').append('<br />');
            }
            $('#capturedPieces').append($(this).clone());
            $('#capturedPieces img').draggable('disable');
          }

          var performCastling = function (from, to) {
            boardConfiguration[to] = boardConfiguration[from];
            delete boardConfiguration[from];
            var rookFromCellId = resolveCellId(from),
                rookToCellId = resolveCellId(to);
            $('#' + rookFromCellId + ' img').attr('src', resolveImageUri('blank'));
            $('#' + rookToCellId + ' img').attr('src', resolveImageUri(currentPlayer + 'r'));
          },
              castlingColumn = currentPlayer === 'w' ? '1' : '8',
              promotionColumn = currentPlayer === 'w' ? '8' : '1';

          if (fromCellName === 'e' + castlingColumn && toCellName === 'c' + castlingColumn && boardConfiguration['e' + castlingColumn] === currentPlayer + 'k') {
            performCastling('a' + castlingColumn, 'd' + castlingColumn);
          }
          if (fromCellName === 'e' + castlingColumn && toCellName === 'g' + castlingColumn && boardConfiguration['e' + castlingColumn] === currentPlayer + 'k') {
            performCastling('h' + castlingColumn, 'f' + castlingColumn);
          }

          boardConfiguration[toCellName] = boardConfiguration[fromCellName];
          delete boardConfiguration[fromCellName];

          $(this).attr('src', $(fromImg).attr('src'));
          $(fromImg).attr('src', resolveImageUri('blank'));

          if (toCellName.charAt(1) === promotionColumn && boardConfiguration[toCellName].charAt(1) === 'p') {
            var elements = $.makeArray($(['n', 'b', 'r', 'q']).map(function () { return '<img src="' + resolveImageUri(currentPlayer + this) + '" id="' + this + '"/>'; })).join('');

            $('<div id="promoteDiv" dest="' + toCellName + '" style="text-align:center">Click on the piece you want to promote to:<br/>' + elements + '</div>').dialog({
              open: function () { $(".ui-helper-clearfix").remove(); },
                closeOnEscape: false,
                modal: true,
                width: 300
              });
            $('#promoteDiv img').click(function () {
              var pieceName = $(this).attr('id');
              boardConfiguration[toCellName] = currentPlayer + pieceName;
              $('#' + resolveCellId(toCellName) + ' img').attr('src', resolveImageUri(currentPlayer + pieceName));

              $('#promoteDiv').dialog('close');
              $('#promoteDiv').remove();
              var turn = currentPlayer;
              changePlayerTurn();
              resolveLegalMoves(fromCellName + '-' + toCellName + '-' + turn + pieceName);
            });
          }
          else {
            changePlayerTurn();
            resolveLegalMoves(fromCellName + '-' + toCellName);
          }
        }
      });

      changePlayerTurn();
      resolveLegalMoves();
    },
    changePlayerTurn = function () {
      currentPlayer = currentPlayer === 'w' ? 'b' : 'w';
      var text = currentPlayer === 'w' ? "White's turn." : "Black's turn.";
      $('#turn').html(text);
      document.title = 'Chess - ' + text;
    };

  chessEngine.setup = setup;
}(window.chessEngine = window.chessEngine || {}, jQuery, undefined));