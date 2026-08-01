 import { showNotification } from "/js/layout.js";

let hovecard = document.getElementById("hovercard");
let questiopn = document.getElementById("question");
let togglebtn = document.getElementById("togglebtn");
let loginform = document.getElementById("LoginForm");
let loginBtn = document.getElementById("LoginBtn");
const forgetpass = document.getElementById("forgetpass");
const sendotp = document.getElementById("sendotp");
const OTP = document.getElementById("OTP");
const SubmitErrors = document.getElementById("SubmitErrors");

let count = 0;
let registrationform = document.getElementById("RegistrationForm");
let RegBtn = document.getElementById("RegBtn"); 
let PassReset = document.getElementById("passreset");
const ForgetPassCard = document.getElementById("ForgetPassCard");
const cancel = document.getElementById("cancel");
const submit = document.getElementById("submit");
const pass = document.getElementById("pass");
const confirmpass = document.getElementById("confirmpass");



cancel.addEventListener("click", () => ForgetPassCard.classList.add("d-none"));
forgetpass.addEventListener("click", () => ForgetPassCard.classList.remove("d-none"));
sendotp.addEventListener("click", async () => {
    var email = document.getElementById("email").value;
    var result = await fetch(`api/sendotp?email=${encodeURIComponent(email)}`);
    var data = await result.json();
    // console.log(data);
    if (data && data.errors && data.errors.Email) {
        document.getElementById("emailotp").innerHTML = data.errors.Email.errors[0].errorMessage;
    }
    if (data.error == false) {
        PassReset.classList.remove("d-none");
        sendotp.disabled = true;
        letscount(60);
        setTimeout(() => {
            sendotp.disabled = false
            sendotp.innerHTML = "Send OTP";
        }, 60000);
        
    }
    console.log("sdasdas");
});

submit.addEventListener("click", async () => {

    SubmitErrors.innerHTML = "";

    if (OTP.value === "") {
        SubmitErrors.innerHTML = "Enter a Valid OTP";
    }
    else if (Number.isNaN(Number(OTP.value))) {
        SubmitErrors.innerHTML = "Enter a Valid OTP";
    }
    else if (confirmpass.value !== pass.value) {
        SubmitErrors.innerHTML = "Password and Confirm Password Doesn't match";
    }
    else {
        SubmitErrors.innerHTML = "";
        var result = await fetch(`api/Resetpass?Email=${encodeURIComponent(email)}&&Password=${encodeURIComponent(pass.value)}&&OTP=${encodeURIComponent(OTP)}`);
        var data = await result.json();
    }
    
});
function letscount(count) {
    const tick = setInterval(() => {
        count--;
        sendotp.innerHTML = "Wait "+count+"s";
        if (count < 1) {
            clearInterval(tick);
        }
    }, 1000);

}


document
  .getElementById("togglebtn")
  .addEventListener("click", function (event) {
    console.log("Toggle");
    const cardLogin = document.getElementById("card_login");

    if (hovecard.style.left === "50%") {
      hovecard.style.left = "0";
      hovecard.style.borderRadius = "25px 100px 0px 25px";
      questiopn.innerHTML = "Don't have an account?";
      togglebtn.innerHTML = "Sign Up";
      if (cardLogin) cardLogin.classList.remove("register-active");
    } else {
      hovecard.style.left = "50%";
      hovecard.style.borderRadius = "100px 25px 25px 0px";
      questiopn.innerHTML = "Already have an account?";
      togglebtn.innerHTML = "Login";
      if (cardLogin) cardLogin.classList.add("register-active");
    }
  });

// Attach listeners to mobile toggle links
document.querySelectorAll(".mobile-toggle-btn").forEach(btn => {
    btn.addEventListener("click", () => {
        const primaryToggle = document.getElementById("togglebtn");
        if (primaryToggle) {
            primaryToggle.click();
        }
    });
});

if (loginBtn) { loginBtn.addEventListener("click", function (event) {
  //console.log("Login");
  const formData = new FormData(loginform);
  const email = formData.get("Email");
  const password = formData.get("Password");

  fetch("api/islogin", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password }),
  })
    .then((res) => res.json())
    .then((data) => {
        console.log(data);
        document.getElementById("SpanEmail").innerHTML = null;
        document.getElementById("SpanPass").innerHTML = null;

        if (data.errors) {
            console.log(data.errors);
            if (data.errors.Email) {
                document.getElementById("SpanEmail").innerHTML = data.errors.Email.errors[0].errorMessage;
            }
            if (data.errors.Password) {
                document.getElementById("SpanPass").innerHTML = data.errors.Password.errors[0].errorMessage;
            }
        }
        else if (!data.error && data.data != null && (data.data.role == "Owner" || data.data.role == "User")) {
            window.location.href="Home"
        }
        else if (!data.error && data.data.role == "Admin") {
            window.location.href="Admin/Home"
        }
        else {

            showNotification(data);
        }
    })
    .catch((err) => console.log(err));
});
}



if (RegBtn) {
    RegBtn.addEventListener("click", function (event) {
        //console.log("Register");
        const formData = new FormData(registrationform);
        const name = formData.get("name");
        const email = formData.get("email");
        //console.log(name, email);
        fetch("api/Reg", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({  name, email  }),
        })
            .then((res) => res.json())
            .then((data) => {

                //console.log(data);
                document.getElementById("SEmail").innerHTML = null;
                document.getElementById("SName").innerHTML = null;
                if (data.errors && data.Error !=true) {
                    //console.log(data.errors);
                    if (data.errors.Email) {
                        document.getElementById("SEmail").innerHTML = data.errors.Email.errors[0].errorMessage;
                    }
                    if (data.errors.Name) {
                        document.getElementById("SName").innerHTML = data.errors.Name.errors[0].errorMessage;
                    }
                }
                else if (data.status) {
                    showNotification(data);
                    togglebtn.click();
                    notif.style.backgroundColor = "#212529";
                    //console.log("Registration done");
                }
                else {

                    showNotification(data);
                }
            })
            .catch((err) => {
                console.log(err);
            });
    });
}