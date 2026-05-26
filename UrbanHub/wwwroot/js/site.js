// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let notibtn = document.getElementById("notificationbtn");
let refreashbtn = document.getElementById("Refreash");
let list = document.getElementById("notificationList");

if (notibtn) {
  notibtn.addEventListener("click", function () {
    loaddata();
  });
}
if (refreashbtn) {
  refreashbtn.addEventListener("click", function () {
    loaddata();
  });
}


async function loaddata() {
    const response = await fetch("api/Notification");
    console.log(response);
  const data = await response.json();
  console.log(data);

    let container = document.getElementById("notificationList");
    container.innerHTML = "";

    if (data.data == null || data.data.length === 0) {
        container.innerHTML = "<p>No notifications available.</p>";
    } else {

        for (let item of data.data) {
            container.innerHTML += `
               <div class="card border-primary mb-3" style="max-width: 20rem;">
               <div class="card-header">${item.title} <span class=" end-0 badge bg-primary ">${new Date(item.date).toLocaleString()}</span></div> 
               <div class="card-body">
                 <p class="card-text">${item.message}</p>
                 ${item.seen == false ? `<button type="button" buttonid="${item.id}" class="btn getthebtn btn-outline-primary "> 
                 <i class="fa-solid fa-check"></i>Mark as seen</button>` : ""}
               </div>
             </div>`;
        }
    }
}



//let BtnID = document.querySelectorAll(".getthebtn");

//if (BtnID) {
//    console.log("eh!!");
//    BtnID.addEventListener("click", () => {
//        BtnID.Foreach((btn) =>{
//            console.log(BtnID.buttonid);
//        });
//    });
//}



//async function markAsSeen(id) {
//    const response = await fetch(`api/Notification/${id}`, {
//        method: "PUT"
//    });
//    const data = await response.json();
//    console.log(data);
//    //Get a container

//    let container = document.getElementById("notificationList");
//    container.innerHTML = "";
//    for (let item of data.data) {
//        container.innerHTML += `
//               <div class="card border-primary mb-3" style="max-width: 20rem;">
//               <div class="card-header">${item.title} <span class=" end-0 badge bg-primary ">${new Date(item.date).toLocaleString()}</span></div> 
//               <div class="card-body">
//                 <p class="card-text">${item.message}</p>
//                 ${item.seen == false ? `<button type="button" buttonid="${item.id}" class="btn getthebtn btn-outline-primary "> 
//                 <i class="fa-solid fa-check"></i>Mark as seen</button>` : ""}
//               </div>
//             </div>`;
//    }
//}