import json


with open('boundaries.json', 'r') as file:
    geojson_data = json.load(file)
def format_coordinates(geometry):
    if 'coordinates' in geometry:
        if isinstance(geometry['coordinates'], list):
            # Ensure each pair of coordinates is printed on a new line
            for i, coord in enumerate(geometry['coordinates']):
                if isinstance(coord, list):
                    geometry['coordinates'][i] = ', '.join([str(c) for c in coord])  # Format the coordinate pair
        else:
            for sub_geometry in geometry['coordinates']:
                format_coordinates(sub_geometry)
# Iterate over each feature and keep only 'NAME' and 'FID' properties
for feature in geojson_data['features']:
    feature['properties'] = {key: feature['properties'][key] for key in feature['properties'] if key in ['NAME', 'FID']}

    # Format coordinates
    format_coordinates(feature['geometry'])
with open('boundaries.geojson', 'w') as outfile:
    json.dump(geojson_data, outfile, indent=2, separators=(',', ': '))

print("GeoJSON file has been modified and saved as 'boundaries.geojson'.")