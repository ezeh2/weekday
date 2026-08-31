"use strict";

const test = require("node:test");
const assert = require("node:assert/strict");
const generator = require("../src/WeekDayWebApplication/WeekDayWebApplication/wwwroot/js/password-generator.js");

const masterPassword = "Synthetic-Only-42!";

test("normalizes URLs to an exact hostname", () => {
    assert.equal(generator.normalizeHostname("HTTPS://Login.Example.COM:443/account"), "login.example.com");
    assert.equal(generator.normalizeHostname("example.com"), "example.com");
});

test("rejects malformed and non-HTTP addresses", () => {
    assert.throws(() => generator.normalizeHostname(""), /site address/i);
    assert.throws(() => generator.normalizeHostname("not a url"), /valid HTTP or HTTPS/i);
    assert.throws(() => generator.normalizeHostname("javascript:alert(1)"), /HTTP or HTTPS/i);
    assert.throws(() => generator.normalizeHostname("https://user:secret@example.com"), /credentials/i);
});

test("separates independently controlled Azure App Service hostnames", async () => {
    const weekday = await generator.derivePassword("https://weekday.azurewebsites.net", masterPassword);
    const attacker = await generator.derivePassword("https://attacker.azurewebsites.net", masterPassword);
    assert.notEqual(weekday, attacker);
});

test("generates deterministic fixed-length passwords with required character classes", async () => {
    const first = await generator.derivePassword("https://example.com", masterPassword);
    const second = await generator.derivePassword("https://example.com/path?ignored=yes", masterPassword);
    assert.equal(first, second);
    assert.equal(first, "jTh8uBhKfiZz6HF5nksY53L4noWtRs8@");
    assert.equal(first.length, 32);
    assert.match(first, /[A-Z]/);
    assert.match(first, /[a-z]/);
    assert.match(first, /[0-9]/);
    assert.match(first, /[!@#$%^&*]/);
});

test("uses full UTF-8 password input without the legacy collisions", async () => {
    const accented = await generator.derivePassword("https://example.com", "long-password-á");
    const differentCodePoint = await generator.derivePassword("https://example.com", "long-password-ǡ");
    const plain = await generator.derivePassword("https://example.com", "Test-password-42!");
    const prefixed = await generator.derivePassword("https://example.com", "@@Test-password-42!");
    assert.notEqual(accented, differentCodePoint);
    assert.notEqual(plain, prefixed);
});

test("requires a meaningful master-password length", async () => {
    await assert.rejects(generator.derivePassword("https://example.com", "short"), /at least 12/i);
});
