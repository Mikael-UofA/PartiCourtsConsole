'use strict'

var map = L.map('map').setView([39.8283, -98.5795], 5);
const geojsonDCPath = '../PrepareData/sources/dc_usable.geojson';
const geojsonCCPath = '../PrepareData/sources/cc_usable.geojson';
const colorMapping = {
    '1': 'blue',
    '-1': 'red', 
    '0': 'purple'
};
const colorMapping2 = {
    '0': '#FFFFFF',
    '1': '#818089', 
    '2': '#F53778',
};
let currentMode = "PARTISANSHIP";
let currentColorMapping = colorMapping;
let currentGeoPath = geojsonDCPath;

const baseLayer = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
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

            if (courtType.textContent === "District Courts") {
                currentGeoPath = geojsonDCPath;
            } else {
                currentGeoPath = geojsonCCPath;
            }
            clearMap();
            loadGeoJSON();
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

            if (modeType.textContent === "Partisanship") {
                currentMode = modeType.textContent.toUpperCase();
                currentColorMapping = colorMapping;
            } else if (modeType.textContent === "Vacancies") {
                currentMode = modeType.textContent.toUpperCase();
                currentColorMapping = colorMapping2;
            }
            clearMap();
            loadGeoJSON();
        })
    })
});

function clearMap() {
    map.eachLayer(layer => {
        if (layer !== baseLayer) {
            map.removeLayer(layer);
        }
    });
}

// Function to create a popup for each feature
function onEachFeature(feature, layer) {
    var content = '';
    if (currentGeoPath === geojsonDCPath) {
        content = `<h3>${feature.properties.NAME}</h3>
        <p>FID: ${feature.properties.FID}</p>
        <p>CHIEF JUDGE: ${feature.properties.CHIEF_JUDGE}</p>
        <p>ACTIVE JUDGES: ${feature.properties.ACTIVE_JUDGES}</p>
        <p>VACANCIES: ${feature.properties.VACANCIES}</p>
        <p>SENIOR ELIGIBLE JUDGES: ${feature.properties.SENIOR_ELIGIBLE_JUDGES}</p>`;
    } else {
        content = `<h3>${feature.properties.NAME}</h3>
                   <p>SUPERVISING JUSTICE: ${feature.properties.SUPERVISING_JUSTICE}</p>
                   <p>CHIEF JUDGE: ${feature.properties.CHIEF_JUDGE}</p>
                   <p>ACTIVE JUDGES: ${feature.properties.ACTIVE_JUDGES}</p>
                   <p>VACANCIES: ${feature.properties.VACANCIES}</p>
                   <p>SENIOR ELIGIBLE JUDGES: ${feature.properties.SENIOR_ELIGIBLE_JUDGES}</p>`;
    }
    layer.bindPopup(content);
}

function loadGeoJSON() {
    fetch(currentGeoPath)
        .then(response => response.json())
        .then(data => {
            L.geoJSON(data, {
                style: function (feature) {
                    return {
                        fillColor: currentColorMapping[feature.properties[currentMode]] || '#9C0B0A',
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

