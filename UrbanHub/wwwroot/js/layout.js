let message = document.getElementById("notifMessage");
let notif = document.getElementById("Notification");

let type = document.getElementById("notifType");

//showNotification({ type: "error", message: 'Welcome to our website!' });

export function showNotification(payload) {
  if ((notif.style.display = "none")) {
    if (payload) {
        if (payload.status == false) {
            notif.style.background = "#9B0F06";
            message.innerHTML = payload.message;
            notif.style.display = "block";
            notif.style.animation = "fadein 1.0s, fadeout .5s 2.5s";
            notif.style.right = "10px";
        }
        else {
            notif.style.background = "#212529";
            message.innerHTML = payload.message;
            notif.style.display = "block";
            notif.style.animation = "fadein 1.0s, fadeout .5s 2.5s";
            notif.style.right = "10px";
        }
      setTimeout(() => {
        notif.style.display = "none";
      }, 10000);
    }
  } else {
    notif.style.display = "none";
    showNotification(payload);
  }
}
