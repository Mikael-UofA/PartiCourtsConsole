import json

# Load GeoJSON file
with open("PrepareData\sources\dc_boundaries.geojson", "r", encoding="utf-8") as f:
    geojson_data = json.load(f)

# Extract FID and NAME
with open("output.txt", "w", encoding="utf-8") as f:
    for feature in geojson_data["features"]:
        fid = feature["properties"].get("FID")
        name = feature["properties"].get("NAME")
        if fid is not None and name is not None:
            f.write(f"{fid} {name}\n")

print("Data has been written to output.txt")