const participationClassification = {
    main: { id: 1, name: "本戦" },
    qualifying: { id: 2, name: "予選" },
    notParticipate: { id: 3, name: "不出場" },
};
const gameStatus = {
    none: { id: 1, name: "未" },
    done: { id: 2, name: "済" },
    notPlayed: { id: 3, name: "NotPlayed" },
    walkover: { id: 4, name: "Walkover" },
    notReadied: { id: 5, name: "準備未完了" },
};
const playerClassification = {
    seed: { id: 1, name: "シード" },
    general: { id: 2, name: "一般" },
    bye: { id: 3, name: "BYE" },
};
const createTournament = (numberOfDraws, assignedPlayers, isSingles) => {
    const numberOfRows = numberOfDraws * 4 - 2;
    const numberOfRounds = Math.log2(numberOfDraws);
    const table = document.createElement("table");
    table.className = "draw-table";

    let drawNumber = 1;
    for (let rowIndex = 0; rowIndex < numberOfRows; rowIndex++) {
        const tr = document.createElement("tr");

        // プレイヤー名表示欄
        if (rowIndex % 2 === 0) {
            const td = document.createElement("td");
            td.setAttribute("rowspan", "2");

            const assignedPlayer = assignedPlayers.find(o => o.drawNumber === drawNumber);
            if (rowIndex % 4 === 0) {
                const playerNames = (() => {

                    if (typeof assignedPlayer === "undefined") {
                        return "";
                    }

                    if (assignedPlayer.playerClassification === playerClassification.bye.id) {
                        return playerClassification.bye.name;
                    }

                    return assignedPlayer.playerNames ? assignedPlayer.playerNames.join("\n") : "";
                })();

                const playerbox = document.createElement("div");
                playerbox.innerText = playerNames;
                playerbox.classList.add("player-box");
                if (isSingles) {
                    playerbox.classList.add("singles");
                }
                if (assignedPlayer && assignedPlayer.framePlayerClassification === playerClassification.seed.id) {
                    playerbox.classList.add("seed-frame");
                }
                if (assignedPlayer && assignedPlayer.framePlayerClassification === playerClassification.bye.id) {
                    playerbox.classList.add("bye-frame");
                }
                playerbox.dataset.drawNumber = drawNumber;
                td.appendChild(playerbox);
                td.classList.add("player-box");
                td.id = `drawNumber${drawNumber}`;
                if (isSingles) {
                    td.classList.add("singles");
                }
                drawNumber++;
            }

            tr.appendChild(td);
        }

        // トーナメント表部分
        let horizontalAdjuster = 0;
        let verticalAdjuster = 0;
        let totalHorizontalAdjuster = 4;
        let totalVerticalAdjuster = 4;
        let divisor = 4;
        for (let roundIndex = 0; roundIndex <= numberOfRounds; roundIndex++) {
            const td = document.createElement("td");

            if ((rowIndex + totalHorizontalAdjuster) % divisor === 0) {
                td.classList.add("underline-short");
            }

            if (roundIndex !== 0 && (rowIndex + totalVerticalAdjuster) % divisor < (divisor / 2)) {
                td.classList.add("sideline");
            }

            if (roundIndex === 0 && (rowIndex + totalVerticalAdjuster) % divisor < (divisor / 2)) {
                td.classList.add("base-height");
                if (isSingles) {
                    td.classList.add("singles");
                }
            }

            tr.appendChild(td);

            if (roundIndex === 0) {
                horizontalAdjuster = 2;
                verticalAdjuster = 3;
            } else {
                horizontalAdjuster *= 2;
                verticalAdjuster *= 2;
            }

            totalHorizontalAdjuster += horizontalAdjuster;
            totalVerticalAdjuster += verticalAdjuster;
            divisor *= 2;
        }

        table.appendChild(tr);
    }

    return table;
}
const updateResult = (table, numberOfDraws, assignedPlayers, gameResults) => {
    const numberOfRows = numberOfDraws * 4 - 2;
    const numberOfRounds = Math.log2(numberOfDraws);
    const firstRound = 1;
    let resultPositions = [];
    for (let rowIndex = 0; rowIndex < numberOfRows; rowIndex++) {
        let horizontalAdjuster = 0;
        let totalHorizontalAdjuster = 4;
        let divisor = 4;

        for (let roundIndex = 0; roundIndex <= numberOfRounds; roundIndex++) {
            if (roundIndex >= firstRound) {
                if ((rowIndex + totalHorizontalAdjuster) % divisor === 0) {
                    // 結果入力リンク
                    const resultPosition = `${`${roundIndex}`.padStart(3, '0')}${`${rowIndex}`.padStart(3, '0')}`;
                    table.childNodes[rowIndex - 1].childNodes[roundIndex].setAttribute("rowspan", "2");
                    if (roundIndex === 0) {
                        table.childNodes[rowIndex - 1].childNodes[roundIndex].classList.add("underline-short");
                    } else {
                        table.childNodes[rowIndex - 1].childNodes[roundIndex].classList.add("underline");
                    }
                    table.childNodes[rowIndex - 1].childNodes[roundIndex].classList.remove("sideline");
                    table.childNodes[rowIndex - 1].childNodes[roundIndex].classList.add("sideline-result");
                    table.childNodes[rowIndex - 1].childNodes[roundIndex].dataset["position"] = resultPosition;
                    table.childNodes[rowIndex].childNodes[roundIndex + 1].remove();
                    resultPositions.push({ resultPosition, rowIndex: rowIndex - 1, roundIndex });

                    // スコア表示欄
                    table.childNodes[rowIndex + 1].childNodes[roundIndex].setAttribute("rowspan", "2");
                    table.childNodes[rowIndex + 1].childNodes[roundIndex].classList.remove("sideline");
                    table.childNodes[rowIndex + 1].childNodes[roundIndex].classList.add("sideline-result");
                    table.childNodes[rowIndex + 2].childNodes[roundIndex + 1].remove();
                }
            }

            if (roundIndex === 0) {
                horizontalAdjuster = 2;
            } else {
                horizontalAdjuster *= 2;
            }

            totalHorizontalAdjuster += horizontalAdjuster;
            divisor *= 2;
        }
    }

    resultPositions.sort((a, b) => Number(a.resultPosition) - Number(b.resultPosition));
    let gameNumber = 0;
    for (const resultPosition of resultPositions) {
        gameNumber++;
        const rowIndex = resultPosition.rowIndex;
        const roundIndex = resultPosition.roundIndex;
        const gameResult = gameResults.find(o => o.gameNumber === gameNumber);
        if (typeof gameResult === "undefined") {
            continue;
        }

        const playerNames = (() => {
            const assignedPlayer = assignedPlayers.find(o => o.entryNumber === gameResult.entryNumberOfWinner);
            if (typeof assignedPlayer === "undefined") {
                return "";
            }

            if (assignedPlayer.playerClassification === playerClassification.bye.id) {
                return playerClassification.bye.name;
            }

            return assignedPlayer.playerNames ? assignedPlayer.playerNames.join("\n") : "";
        })();

        if (gameResult) {
            if (gameResult.gameStatus !== gameStatus.notReadied.id) {
                table.childNodes[rowIndex].childNodes[roundIndex].classList.add("result-input");
            }

            switch (gameResult.gameStatus) {
                case gameStatus.none.id:
                    table.childNodes[rowIndex].childNodes[roundIndex].innerText = "（入力）";
                    table.childNodes[rowIndex + 2].childNodes[roundIndex].innerText = "";
                    break;
                case gameStatus.done.id:
                    table.childNodes[rowIndex].childNodes[roundIndex].innerText = playerNames;
                    table.childNodes[rowIndex + 2].childNodes[roundIndex].innerText = gameResult.gameScore;
                    break;
                case gameStatus.notPlayed.id:
                    table.childNodes[rowIndex].childNodes[roundIndex].innerText = gameStatus.notPlayed.name;
                    table.childNodes[rowIndex + 2].childNodes[roundIndex].innerText = "";
                    break;
                case gameStatus.walkover.id:
                    table.childNodes[rowIndex].childNodes[roundIndex].innerText = playerNames;
                    table.childNodes[rowIndex + 2].childNodes[roundIndex].innerText = gameStatus.walkover.name;
                    break;
                case gameStatus.notReadied.id:
                    continue;
            }

            table.childNodes[rowIndex].childNodes[roundIndex].addEventListener("click", async (ev) => {

                showInputResultDialog(gameResult);

            }, false);
        }
    }
}
