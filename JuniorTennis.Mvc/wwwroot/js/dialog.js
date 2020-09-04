const openDialog = id => {
    const dialog = document.getElementById(id + "Dialog");
    dialog.showModal();
}
const showDialog = elementId => {
    var dialogBackground = document.createElement("div");
    dialogBackground.id = "DialogBackground";
    dialogBackground.className = "dialog-background";
    document.querySelector("main").appendChild(dialogBackground);

    document.getElementById(elementId).classList.add("open");
}
const closeDialog = elementId => {
    document.getElementById(elementId).classList.remove("open");

    var dialogBackground = document.getElementById("DialogBackground");
    document.querySelector("main").removeChild(dialogBackground);
}
