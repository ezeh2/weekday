"use strict";

document.addEventListener("DOMContentLoaded", () => {
    const sendButton = document.querySelector("#send");
    const dataInput = document.querySelector("#data");

    if (!sendButton || !dataInput) {
        return;
    }

    sendButton.addEventListener("click", async () => {
        const url = new URL("/Data/SaveData", window.location.origin);
        url.searchParams.set("data", dataInput.value);

        try {
            const response = await fetch(url, { cache: "no-store" });
            if (!response.ok) {
                throw new Error(`Request failed with status ${response.status}.`);
            }
        } catch (error) {
            console.error("Unable to save data.", error);
        }
    });
});
