(function () {
    "use strict";

    document.addEventListener("DOMContentLoaded", () => {
        const generator = window.WeekdayPasswordGenerator;
        const form = document.querySelector("#passwordGenerator");
        if (!generator || !form) {
            return;
        }

        const domain = document.querySelector("#theSiteDomain");
        const masterPassword = document.querySelector("#theSitePassword");
        const output = document.querySelector("#theHashedPassword");
        const generateButton = document.querySelector("#generate");
        const copyButton = document.querySelector("#copy");
        const visibilityButton = document.querySelector("#toggleVisibility");
        const status = document.querySelector("#generatorStatus");

        function setOutput(value) {
            output.value = value;
            copyButton.disabled = !value;
            visibilityButton.disabled = !value;
        }

        form.addEventListener("submit", async (event) => {
            event.preventDefault();
            generateButton.disabled = true;
            setOutput("");
            status.textContent = "Deriving password…";

            try {
                const value = await generator.derivePassword(domain.value, masterPassword.value);
                setOutput(value);
                masterPassword.value = "";
                status.textContent = `Generated a ${generator.OUTPUT_LENGTH}-character password locally.`;
            } catch (error) {
                status.textContent = error instanceof Error ? error.message : "Password generation failed.";
            } finally {
                generateButton.disabled = false;
            }
        });

        copyButton.addEventListener("click", async () => {
            try {
                await navigator.clipboard.writeText(output.value);
                status.textContent = "Copied to the clipboard.";
            } catch {
                status.textContent = "Clipboard access was blocked; show and copy the password manually.";
            }
        });

        visibilityButton.addEventListener("click", () => {
            const show = output.type === "password";
            output.type = show ? "text" : "password";
            visibilityButton.textContent = show ? "Hide" : "Show";
        });
    });
}());
