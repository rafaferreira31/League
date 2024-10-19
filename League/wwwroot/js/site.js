function checkGameDate() {
    const gameDateInput = document.getElementById("gameDate");
    const visitedGoalsGroup = document.getElementById("visitedGoalsGroup");
    const visitorGoalsGroup = document.getElementById("visitorGoalsGroup");
    const visitedAssignedCardsGroup = document.getElementById("visitedAssignedCardsGroup");
    const visitorAssignedCardsGroup = document.getElementById("visitorAssignedCardsGroup");

    const gameDate = new Date(gameDateInput.value);
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (gameDate < today) {
        visitedGoalsGroup.style.display = "block";
        visitorGoalsGroup.style.display = "block";
        visitedAssignedCardsGroup.style.display = "block";
        visitorAssignedCardsGroup.style.display = "block";
    } else {
        visitedGoalsGroup.style.display = "none";
        visitorGoalsGroup.style.display = "none";
        visitedAssignedCardsGroup.style.display = "none";
        visitorAssignedCardsGroup.style.display = "none";
    }
}

function checkClubs() {
    const visitedClub = document.getElementById("visitedClub").value;
    const visitorClub = document.getElementById("visitorClub").value;

    if (visitedClub === visitorClub) {
        alert("The Home Club and Away Club cannot be the same.");
        document.getElementById("visitorClub").selectedIndex = 0;
    }
}

document.getElementById("gameDate").addEventListener("change", checkGameDate);
document.getElementById("visitedClub").addEventListener("change", checkClubs);
document.getElementById("visitorClub").addEventListener("change", checkClubs);

window.onload = checkGameDate;
