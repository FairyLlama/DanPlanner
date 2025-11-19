// wwwroot/js/map.js
window.showCampingSite = function (site) {
    console.log("[map.js] showCampingSite CALLED", site);

    if (typeof L === "undefined") {
        console.error("[map.js] Leaflet L is undefined");
        return;
    }

    const el = document.getElementById('map');
    console.log("[map.js] map element height:", getComputedStyle(el).height);

    if (window._map) {
        try { window._map.remove(); } catch (e) { console.warn("remove failed:", e); }
        window._map = null;
    }

    const lat = parseFloat(site.latitude ?? site.Latitude);
    const lon = parseFloat(site.longitude ?? site.Longitude);
    console.log("[map.js] parsed coords:", lat, lon);

    if (!Number.isFinite(lat) || !Number.isFinite(lon)) {
        console.error("[map.js] invalid coords", site);
        return;
    }

    window._map = L.map('map').setView([lat, lon], 15);

    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap'
    }).addTo(window._map);

    L.marker([lat, lon])
        .addTo(window._map)
        .bindPopup(`<b>${site.name ?? site.Name}</b><br>${site.region ?? site.Region}<br>${site.address ?? site.Address}`)
        .openPopup();

    console.log("[map.js] map initialized at", { lat, lon });
};