'use strict';

// Initialize Map
const map = L.map('map').setView([39.8283, -98.5795], 5);
const baseLayer = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

// GeoJSON Paths and Settings
const geojsonPaths = {
    district: 'sources/dc_usable.geojson',
    circuit: 'sources/cc_usable.geojson'
};
const colorMappings = {
    partisanship: { '1': 'blue', '-1': 'red', '0': 'purple' },
    vacancies: { '0': '#FFFFFF', '1': '#818089', '2': '#F53778' }
};
const defaultColors = '#9C0B0A'; // Default fallback color
let currentSettings = {
    mode: 'PARTISANSHIP',
    geoPath: geojsonPaths.district,
    colorMapping: colorMappings.partisanship
};
// DOM Elements
const elements = {
    aboutLink: document.querySelector('.about-popup'),
    modalPopup: document.querySelector('.window'),
    modalCloseBtn: document.querySelector('.window__btn'),
    courtTypeDisplay: document.querySelector('.court-display'),
    modeDisplay: document.querySelector('.mode-display'),
    courtLinks: document.querySelectorAll('.court-type a'),
    modeLinks: document.querySelectorAll('.mode-type a'),
    legends: {
        legend1: document.getElementById('legend1'),
        legend2: document.getElementById('legend2')
    }
};

// Event Listeners
document.addEventListener('DOMContentLoaded', () => {
    initializeMap();
    setupEventListeners();
});

// Initialize Map with GeoJSON
function initializeMap() {
    loadGeoJSON(currentSettings.geoPath);
}
// Event Listener Setup
function setupEventListeners() {
    // About Popup Handlers
    elements.aboutLink.addEventListener('click', () => togglePopup(false));
    elements.modalCloseBtn.addEventListener('click', () => togglePopup(true));

    // Court Type Handlers
    elements.courtLinks.forEach(link => {
        link.addEventListener('click', event => handleCourtTypeChange(event, link));
    });
    // Mode Handlers
    elements.modeLinks.forEach(link => {
        link.addEventListener('click', event => handleModeChange(event, link));
    });
}

// Toggle Popup
function togglePopup(hide) {
    elements.modalPopup.classList.toggle('hidden', hide);
}

// Handle Court Type Change
function handleCourtTypeChange(event, link) {
    event.preventDefault();
    if (link.classList.contains('selected')) return;

    updateSelection(elements.courtLinks, link);
    elements.courtTypeDisplay.textContent = link.textContent;

    currentSettings.geoPath = link.textContent === 'District Courts' ? geojsonPaths.district : geojsonPaths.circuit;
    refreshMap();
}

// Handle Mode Change
function handleModeChange(event, link) {
    event.preventDefault();
    if (link.classList.contains('selected')) return;

    updateSelection(elements.modeLinks, link);
    elements.modeDisplay.textContent = link.textContent;

    currentSettings.mode = link.textContent.toUpperCase();
    currentSettings.colorMapping = getColorMapping(currentSettings.mode);

    updateLegend(currentSettings.mode);
    refreshMap();
}

// Update Selected State
function updateSelection(links, selectedLink) {
    links.forEach(link => link.classList.remove('selected', 'unclickable'));
    selectedLink.classList.add('selected', 'unclickable');
}

// Get Color Mapping for Current Mode
function getColorMapping(mode) {
    switch (mode) {
        case 'VACANCIES': return colorMappings.vacancies;
        default: return colorMappings.partisanship;
    }
}

// Update Legend Visibility
function updateLegend(mode) {
    const { legend1, legend2 } = elements.legends;
    legend1.classList.toggle('hidden', mode !== 'PARTISANSHIP');
    legend2.classList.toggle('hidden', mode === 'PARTISANSHIP');
}

// Clear Map Layers
function clearMap() {
    map.eachLayer(layer => {
        if (layer !== baseLayer) map.removeLayer(layer);
    });
}

// Refresh Map with Updated GeoJSON
function refreshMap() {
    clearMap();
    loadGeoJSON(currentSettings.geoPath);
}

// Load GeoJSON Data and Style Features
function loadGeoJSON(path) {
    fetch(path)
        .then(response => response.json())
        .then(data => {
            L.geoJSON(data, {
                style: feature => ({
                    fillColor: currentSettings.colorMapping[getColoringProperty(feature)] || defaultColors,
                    weight: 2,
                    color: '#000000',
                    opacity: 1,
                    fillOpacity: 0.5
                }),
                onEachFeature: bindFeaturePopup
            }).addTo(map);
        })
        .catch(error => console.error('Error loading GeoJSON:', error));
}

// Determine Feature Coloring Property
function getColoringProperty(feature) {
    return currentSettings.mode === 'NO VACANCIES'
        ? getPartisanshipIfFilled(feature)
        : feature.properties[currentSettings.mode];
}

// Calculate Partisanship if Filled
function getPartisanshipIfFilled(feature) {
    const partisanship = feature.properties.DEMJUDGES - feature.properties.GOPJUDGES + 
        (feature.properties.VACANCIES * (currentSettings.currentPresident || 0));

    return partisanship > 0 ? 1 : partisanship < 0 ? -1 : 0;
}

function bindFeaturePopup(feature, layer) {
    const { NAME, FID, CHIEF_JUDGE, ACTIVE_JUDGES, VACANCIES, SENIOR_ELIGIBLE_JUDGES, SUPERVISING_JUSTICE } = feature.properties;

    const content = currentSettings.geoPath === geojsonPaths.district
        ? `<h3>${NAME}</h3><p>FID: ${FID}</p><p>CHIEF JUDGE: ${CHIEF_JUDGE}</p><p>ACTIVE JUDGES: ${ACTIVE_JUDGES}</p><p>VACANCIES: ${VACANCIES}</p><p>SENIOR ELIGIBLE JUDGES: ${SENIOR_ELIGIBLE_JUDGES}</p>`
        : `<h3>${NAME}</h3><p>SUPERVISING JUSTICE: ${SUPERVISING_JUSTICE}</p><p>CHIEF JUDGE: ${CHIEF_JUDGE}</p><p>ACTIVE JUDGES: ${ACTIVE_JUDGES}</p><p>VACANCIES: ${VACANCIES}</p><p>SENIOR ELIGIBLE JUDGES: ${SENIOR_ELIGIBLE_JUDGES}</p>`;
    
    layer.bindPopup(content);
}

