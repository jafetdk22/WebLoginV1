"use strict";
var KTApp = function () {
    const body = document.querySelector("body");
    let loaderSection;

    const show = function () {
        loaderSection = document.createElement("section");
        loaderSection.classList.add("loader-animation");

        for (let i = 0; i < 3; i++) {
            const divLoader = document.createElement("div");
            divLoader.classList.add("divLoader");
            loaderSection.appendChild(divLoader);
        }

        body.appendChild(loaderSection);
    };

    const hide = function () {
        if (loaderSection) {
            body.removeChild(loaderSection);
            loaderSection = null;
        }
    };

    return {
        showPageLoading: function () {
            show();
        },
        hidePageLoading: function () {
            hide();
        },
    };
}();
