let terms = document.getElementById('terms');
let submitBtn = document.getElementById('submitbtn');

if (terms) {
    terms.addEventListener('change', function (event) {
        if (!terms.checked) {
            submitBtn.disabled = true;
        } else {
            submitBtn.disabled = false;
        }
    });
}