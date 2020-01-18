/*
 * Remote PwdHash
 * A JavaScript implementation of the PwdHash hashing algorithm.
 * Version 1.0 Copyright (C) Stanford University 2004-2006
 * Author: Collin Jackson
 * Other Contributors: Dan Boneh, John Mitchell, Nick Miyake, and Blake Ross
 * Distributed under the BSD License
 * See http://crypto.stanford.edu/PwdHash for more info.
 * Requires the Javascript MD5 library, available here: http://pajhome.org.uk/crypt/md5
 */

$(document).ready(function () {

    $('#generate').on("click", function (event) {
        ButtonClick_Generate();        
    });
    $('#generateAndCopyToClipboard').on("click", function (event) {
        ButtonClick_GenerateAndCopyToClipboard();
    });

    Init();
});

/*
 * Initialize page with default hashing parameters.
 */
function Init() {
    document.hashform.domain.value = "http://www.example.com/";
    document.hashform.sitePassword.value = "";
    document.hashform.hashedPassword.value = "Press Generate";
    document.hashform.hashedPassword.disabled = true;
}

var SPH_kPasswordPrefix = "@@";

/*
 * Returns a conforming hashed password generated from the form's field values.
 */
function Generate() {
    var uri = document.hashform.domain.value;
    var domain = (new SPH_DomainExtractor()).extractDomain(uri);
    var size = SPH_kPasswordPrefix.length;
    var data = document.hashform.sitePassword.value;
    if (data.substring(0, size) == SPH_kPasswordPrefix)
        data = data.substring(size);
    var result = new String(new SPH_HashedPassword(data, domain));
    return result;
}

/*
 * Obtain a conforming hashed password and put it in the hashed password field
 */
function ButtonClick_Generate() {
    document.hashform.hashedPassword.value = Generate();
    document.hashform.hashedPassword.disabled = false;
}

function ButtonClick_GenerateAndCopyToClipboard() {
    document.hashform.hashedPassword.value = Generate();
    document.hashform.hashedPassword.disabled = false;

    var copyText = document.getElementById("theHashedPassword");
    /* Select the text field */
    copyText.select();
    copyText.setSelectionRange(0, 99999); /*For mobile devices*/

    /* Copy the text inside the text field */
    document.execCommand("copy");
    document.hashform.hashedPassword.value = '';

}