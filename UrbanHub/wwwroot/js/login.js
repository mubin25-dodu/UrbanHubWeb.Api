import { showNotification } from "/js/layout.js";

let hovecard = document.getElementById("hovercard");
let questiopn = document.getElementById("question");
let togglebtn = document.getElementById("togglebtn");
let loginform = document.getElementById("LoginForm");
let loginBtn = document.getElementById("LoginBtn");

let registrationform = document.getElementById("RegistrationForm");
let RegBtn = document.getElementById("RegBtn");

document
  .getElementById("togglebtn")
  .addEventListener("click", function (event) {
    console.log("Toggle");

    if (hovecard.style.left === "50%") {
      hovecard.style.left = "0";
      hovecard.style.borderRadius = "25px 100px 0px 25px";
      questiopn.innerHTML = "Don't have an account?";
      togglebtn.innerHTML = "Sign Up";
    } else {
      hovecard.style.left = "50%";
      hovecard.style.borderRadius = "100px 25px 25px 0px";
      questiopn.innerHTML = "Already have an account?";
      togglebtn.innerHTML = "Login";
    }
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
        else if (data.status && data.data!=null && (data.data.role == "Owner" || data.data.role == "User") ){
            window.location.href="Home"
        }
        else if (data.status && data.data.role == "Admin") {
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
                notif.style.backgroundColor = "";
                if (data.errors && data.status!=false) {
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