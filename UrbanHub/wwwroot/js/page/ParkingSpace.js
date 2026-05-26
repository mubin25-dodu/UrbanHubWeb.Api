
// IMAGE PREVIEW
const imgInp = document.getElementById('imgInp');
const preview = document.getElementById('preview');

if (imgInp) {

    imgInp.onchange = evt => {

        const [file] = imgInp.files;

        if (file) {

            preview.src = URL.createObjectURL(file);

            preview.classList.remove('d-none');
        }
    }
}


// SCHEDULE JSON
let schedules = [];

function addSchedule() {

    const day =
        document.getElementById('day').value;

    const startTime =
        document.getElementById('startTime').value;

    const endTime =
        document.getElementById('endTime').value;

    // VALIDATION
    if (!startTime || !endTime) {

        document.getElementById('scheduleError')
            .innerText =
            "Start time and end time are required.";

        return;
    }

    document.getElementById('scheduleError')
        .innerText = "";

    // JSON OBJECT
    const data = {

        Day: day,
        StartTime: startTime,
        EndTime: endTime
    };

    schedules.push(data);

    updateScheduleUI();
}

function updateScheduleUI() {

    const scheduleList =
        document.getElementById('scheduleList');

    scheduleList.innerHTML = "";

    schedules.forEach((item, index) => {

        scheduleList.innerHTML += `

            <div class="d-flex
                        justify-content-between
                        align-items-center
                        border
                        rounded-4
                        p-3
                        mb-2">

                <div>

                    <strong>${item.Day}</strong>

                    <div class="text-muted">
                        ${item.StartTime} - ${item.EndTime}
                    </div>

                </div>

                <button type="button"
                        class="btn btn-sm btn-danger"
                        onclick="removeSchedule(${index})">

                    Remove

                </button>

            </div>
        `;
    });

    // SAVE JSON STRING
    document.getElementById('Available').value =
        JSON.stringify(schedules);
}

function removeSchedule(index) {

    schedules.splice(index, 1);

    updateScheduleUI();
}


// REQUIRED VALIDATION
document.querySelector("form")
    .addEventListener("submit", function (e) {

        if (schedules.length === 0) {

            e.preventDefault();

            const error =
                document.getElementById('scheduleError');

            error.innerText =
                "Available schedule is required.";

            // GO TO ERROR SECTION
            error.scrollIntoView({

                behavior: "smooth",
                block: "center"
            });
        }
    });


// MAP
let map;
let marker;

function initializeMap(lat, lng) {

    map = L.map('map').setView([lat, lng], 15);

    L.tileLayer(
        'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
        {
            attribution: '© OpenStreetMap Contributors'
        }
    ).addTo(map);

    marker = L.marker(
        [lat, lng],
        {
            draggable: true
        }
    ).addTo(map);

    marker.bindPopup("Parking Location")
        .openPopup();

    updateLocation(lat, lng);

    // CLICK EVENT
    map.on('click', function (e) {

        const clickedLat = e.latlng.lat;

        const clickedLng = e.latlng.lng;

        marker.setLatLng([clickedLat, clickedLng]);

        updateLocation(clickedLat, clickedLng);
    });

    // DRAG EVENT
    marker.on('dragend', function () {

        const position = marker.getLatLng();

        updateLocation(
            position.lat,
            position.lng
        );
    });

    // FIX MAP SIZE
    setTimeout(() => {

        map.invalidateSize();

    }, 300);
}

function updateLocation(lat, lng) {

    document.getElementById('latitude').value = lat;

    document.getElementById('longitude').value = lng;

    document.getElementById('latText').innerText =
        lat.toFixed(6);

    document.getElementById('lngText').innerText =
        lng.toFixed(6);
}


// GET USER LOCATION
if (navigator.geolocation) {

    navigator.geolocation.getCurrentPosition(

        function (position) {

            initializeMap(
                position.coords.latitude,
                position.coords.longitude
            );
        },

        function () {

            // DEFAULT DHAKA
            initializeMap(
                23.8103,
                90.4125
            );
        }
    );
}
else {

    initializeMap(
        23.8103,
        90.4125
    );
}