'use strict'

// Initialize map and set its view
var map = L.map('map').setView([39.8283, -98.5795], 5);
const geojsonPath = '../PrepareData/sources/usable.geojson';
const colorMapping = {
    '1': 'blue',
    '-1': 'red', 
    '0': 'purple'
};

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { // Add tile layer
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

document.addEventListener('DOMContentLoaded', () => {
    loadGeoJSON();
    const aboutLink = document.querySelector('.about-popup')
    const currentCourtType = document.querySelector('.court-display');
    const currentModeType = document.querySelector('.mode-display');
    const courtTypes = document.querySelectorAll('.court-type a');
    const modeTypes = document.querySelectorAll('.mode-type a');
    const modalPopup = document.querySelector('.window');
    const modalCloseBtn = document.querySelector('.window__btn');


    aboutLink.addEventListener('click', () => {
        modalPopup.classList.remove('hidden');
    })
    modalCloseBtn.addEventListener('click', () => {
        modalPopup.classList.add('hidden');
    })
    courtTypes.forEach(courtType => {
        courtType.addEventListener('click', function(event) {
            // If link is already selected, prevent further clicks
            if (courtType.classList.contains('selected')) {
                event.preventDefault();
                return;
            }
            // Prevent navigation
            event.preventDefault();
            currentCourtType.textContent = courtType.textContent;
            courtTypes.forEach(item => {
                item.classList.remove('selected');
                item.classList.remove('unclickable');
            });
            courtType.classList.add('selected');
            courtType.classList.add('unclickable');
        })

    })

    modeTypes.forEach(modeType => {
        modeType.addEventListener('click', function(event) {
            // If link is already selected, prevent further clicks
            if (modeType.classList.contains('selected')) {
                event.preventDefault();
                return;
            }
            // Prevent navigation
            event.preventDefault();

            currentModeType.textContent = modeType.textContent;
            modeTypes.forEach(item => {
                item.classList.remove('selected');
                item.classList.remove('unclickable');
            });
            modeType.classList.add('selected');
            modeType.classList.add('unclickable');
        })
    })
});

// Function to style features based on PARTISANSHIP property
function getColor(partisanship) {
    return partisanship === 1 ? 'blue' : partisanship === -1 ? 'red' : 'purple';
}
// Function to create a popup for each feature
function onEachFeature(feature, layer) {
    var content = `<h3>${feature.properties.NAME}</h3>
                   <p>FID: ${feature.properties.FID}</p>
                   <p>CHIEF JUDGE: ${feature.properties.CHIEF_JUDGE}</p>
                   <p>ACTIVE JUDGES: ${feature.properties.ACTIVE_JUDGES}</p>
                   <p>SENIOR ELIGIBLE JUDGES: ${feature.properties.SENIOR_ELIGIBLE_JUDGES}</p>`;
    layer.bindPopup(content);
}

function loadGeoJSON() {
    fetch('../PrepareData/sources/usable.geojson')
        .then(response => response.json())
        .then(data => {
            L.geoJSON(data, {
                style: function (feature) {
                    return {
                        fillColor: colorMapping[feature.properties.PARTISANSHIP] || '#FFFFFF',
                        weight: 2,
                        color: '#000000',
                        opacity: 1,                   
                        fillOpacity: 0.5
                    };
                },
                onEachFeature: onEachFeature
            }).addTo(map);
        })
        .catch(error => {
            console.error("Error loading GeoJSON data:", error);
        });
}
