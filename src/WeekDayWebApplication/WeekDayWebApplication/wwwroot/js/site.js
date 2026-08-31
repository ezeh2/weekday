"use strict";

document.addEventListener("DOMContentLoaded", () => {
    const consentButton = document.querySelector("#cookieConsent button[data-cookie-string]");
    if (!consentButton) {
        return;
    }

    consentButton.addEventListener("click", () => {
        document.cookie = consentButton.dataset.cookieString;
        document.querySelector("#cookieConsent")?.classList.add("hidden");
    });
});
