(function (root, factory) {
    "use strict";

    const api = factory(root);
    if (typeof module === "object" && module.exports) {
        module.exports = api;
    } else {
        root.WeekdayPasswordGenerator = api;
    }
}(typeof globalThis !== "undefined" ? globalThis : this, function (root) {
    "use strict";

    const ITERATIONS = 600000;
    const OUTPUT_LENGTH = 32;
    const MINIMUM_MASTER_PASSWORD_LENGTH = 12;
    const VERSIONED_SALT_PREFIX = "weekday-password-generator-v2\0";
    const UPPERCASE = "ABCDEFGHJKLMNPQRSTUVWXYZ";
    const LOWERCASE = "abcdefghijkmnopqrstuvwxyz";
    const DIGITS = "23456789";
    const SPECIAL = "!@#$%^&*";
    const BASE64_URL = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";

    function getCrypto() {
        if (root.crypto && root.crypto.subtle) {
            return root.crypto;
        }
        if (typeof require === "function") {
            return require("node:crypto").webcrypto;
        }
        throw new Error("The Web Crypto API is not available in this browser.");
    }

    function normalizeHostname(siteAddress) {
        if (typeof siteAddress !== "string" || siteAddress.trim() === "") {
            throw new Error("Enter a site address.");
        }

        let candidate = siteAddress.trim();
        if (!/^[a-z][a-z0-9+.-]*:\/\//i.test(candidate)) {
            candidate = `https://${candidate}`;
        }

        let url;
        try {
            url = new URL(candidate);
        } catch {
            throw new Error("Enter a valid HTTP or HTTPS site address.");
        }

        if (url.protocol !== "http:" && url.protocol !== "https:") {
            throw new Error("Only HTTP and HTTPS site addresses are accepted.");
        }
        if (url.username || url.password) {
            throw new Error("Site addresses must not contain credentials.");
        }

        const hostname = url.hostname.toLowerCase().replace(/\.$/, "");
        if (!hostname) {
            throw new Error("The site address must contain a hostname.");
        }
        return hostname;
    }

    function encodeBase64Url(bytes) {
        let result = "";
        for (let offset = 0; offset < bytes.length; offset += 3) {
            const first = bytes[offset];
            const second = offset + 1 < bytes.length ? bytes[offset + 1] : 0;
            const third = offset + 2 < bytes.length ? bytes[offset + 2] : 0;
            const value = (first << 16) | (second << 8) | third;

            result += BASE64_URL[(value >>> 18) & 63];
            result += BASE64_URL[(value >>> 12) & 63];
            if (offset + 1 < bytes.length) {
                result += BASE64_URL[(value >>> 6) & 63];
            }
            if (offset + 2 < bytes.length) {
                result += BASE64_URL[value & 63];
            }
        }
        return result;
    }

    async function derivePassword(siteAddress, masterPassword) {
        if (typeof masterPassword !== "string" || masterPassword.length < MINIMUM_MASTER_PASSWORD_LENGTH) {
            throw new Error(`Use a master password with at least ${MINIMUM_MASTER_PASSWORD_LENGTH} characters.`);
        }
        if (masterPassword.length > 1024) {
            throw new Error("The master password must not exceed 1024 characters.");
        }

        const hostname = normalizeHostname(siteAddress);
        const encoder = new TextEncoder();
        const cryptoApi = getCrypto();
        const key = await cryptoApi.subtle.importKey(
            "raw",
            encoder.encode(masterPassword),
            "PBKDF2",
            false,
            ["deriveBits"]
        );
        const bits = await cryptoApi.subtle.deriveBits(
            {
                name: "PBKDF2",
                hash: "SHA-256",
                iterations: ITERATIONS,
                salt: encoder.encode(VERSIONED_SALT_PREFIX + hostname)
            },
            key,
            256
        );
        const bytes = new Uint8Array(bits);
        const core = encodeBase64Url(bytes).substring(0, OUTPUT_LENGTH - 4);

        return core
            + UPPERCASE[bytes[28] % UPPERCASE.length]
            + LOWERCASE[bytes[29] % LOWERCASE.length]
            + DIGITS[bytes[30] % DIGITS.length]
            + SPECIAL[bytes[31] % SPECIAL.length];
    }

    return Object.freeze({ ITERATIONS, OUTPUT_LENGTH, derivePassword, normalizeHostname });
}));
