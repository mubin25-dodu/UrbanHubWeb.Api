let entryTime = document.getElementById("entryTime");
let exitTime = document.getElementById("exitTime");
let requestBookingBtn = document.getElementById("requestBookingBtn");
let startingtime = document.getElementsByClassName("startingtime");
let endingtime = document.getElementsByClassName("endingtime");
let RentperHour = parseFloat(document.getElementById("RentperHour").innerText);
function checkAvailability() {
    if (!entryTime.value || !exitTime.value) return;
    console.log("Checking availability...");
    if (new Date(entryTime.value) >= new Date(exitTime.value)) {
        document.getElementById("availabilityStatus").classList.remove("d-none");
        document.getElementById("availabilitySuccess").classList.add("d-none");

        return;
    }

    const requestedStart = new Date(entryTime.value);
    const requestedEnd = new Date(exitTime.value);
    let isConflict = false;
    let hourCount = parseFloat((requestedEnd - requestedStart) / (1000 * 60 * 60));
    console.log(hourCount);
    for (var i = 0; i < startingtime.length; i++) {
        const bookedStart = new Date(startingtime[i].innerText);
        const bookedEnd = new Date(endingtime[i].innerText);

        if (bookedStart < requestedEnd && bookedEnd > requestedStart) {
            isConflict = true;
            break;
        }
    }

    if (isConflict) {
        document.getElementById("availabilityStatus").classList.remove("d-none");
        document.getElementById("availabilitySuccess").classList.add("d-none");
        requestBookingBtn.setAttribute("disabled", "disabled");
    } else {
        document.getElementById("availabilityStatus").classList.add("d-none");
        document.getElementById("availabilitySuccess").classList.remove("d-none");
        document.getElementById("availabilitySuccess").innerHTML = `<div>
        <small class="fw-semibold">This time slot is available!</small>
        <p class="mb-0 small mt-1" id="availableInfo">You can proceed with booking.</p>
        <p>Your booking duration is <span id="bookingDuration">${(hourCount).toFixed(2)}</span> hours.</p>
        <h4>Payable Amount: <span id="payableAmount">${(hourCount * RentperHour ).toFixed(2)}</span> BDT</h4>
        </div>`;
        requestBookingBtn.removeAttribute("disabled");
    }
}

if (entryTime) entryTime.addEventListener("change", checkAvailability);
if (exitTime) exitTime.addEventListener("change", checkAvailability);