window.showCampingSites = (sites) => {
    console.log("[map.js] called with", sites);

    const el = document.getElementById('map');
    if (!el) {
        console.error("[map.js] element #map not found");
        return;
    }

    if (window._map) {
        window._map.remove();
    }

    window._map = L.map('map').setView([56.0, 10.0], 6);

    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap'
    }).addTo(window._map);

    if (!sites || sites.length === 0) {
        L.marker([55.6761, 12.5683]) // fallback i København
            .addTo(window._map)
            .bindPopup("Ingen pladser – testmarkør i København");
        console.log("[map.js] no sites, showing fallback marker");
        return;
    }

    const markers = [];
    sites.forEach(site => {
        const lat = parseFloat(site.latitude ?? site.Latitude);
        const lon = parseFloat(site.longitude ?? site.Longitude);
        const name = site.name ?? site.Name;
        const region = site.region ?? site.Region;

        if (!isNaN(lat) && !isNaN(lon)) {
            const marker = L.marker([lat, lon])
                .addTo(window._map)
                .bindPopup(`<b>${name}</b><br>${region}`);
            markers.push(marker);
        } else {
            console.warn("[map.js] invalid coords", site);
        }
    });

    if (markers.length > 0) {
        const group = new L.featureGroup(markers);
        window._map.fitBounds(group.getBounds().pad(0.2));
    }

    console.log("[map.js] map initialized with", markers.length, "markers");
};