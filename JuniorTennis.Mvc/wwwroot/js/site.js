// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const convertToDom = htmlText => {
    const body = new DOMParser()
        .parseFromString(htmlText, "text/html")
        .querySelector("body");
    if (body.hasChildNodes()) {
        return body.children[0];
    }

    return "";
}

const removeChildren = element => {
    while (element.hasChildNodes()) {
        element.removeChild(element.firstChild);
    }
}

const handleExport = (text, fileName) => {
    const a = document.createElement('a');
    a.href = URL.createObjectURL(new Blob([text], { type: 'text/plain' }));
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
}

const createQueryString = form => {
    const formData = new FormData(form);
    const params = Array.from(formData).filter(([, value]) => value !== "");
    return new URLSearchParams(params).toString();
}
