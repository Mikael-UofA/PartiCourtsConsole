# PartiCourts 🌐⚖️  

**PartiCourts** is a data visualization project that showcases the partisanship of U.S. District and Circuit Courts through an interactive map. By scraping, processing, and presenting data about the courts and judges, this project provides an insightful view of the judiciary's political makeup.  

## Project Overview 🗺️  

The project is structured into three main directories:  

### 1. **PrepareData** 🛠️  
This module handles data acquisition and processing.  
- **Purpose**:  
  Scrapes court and judge information from Wikipedia and processes it to determine the partisanship of each court.  
- **Features**:  
  - Collects data on courts, including:  
    - Number of judges.  
    - Chief judge.  
    - Court name.  
  - Collects judge-specific data, including:  
    - Names.  
    - Birth year.  
    - Appointing president.  
  - Processes data to classify each court as:  
    - Majority Democratic 🟦.  
    - Majority Republican 🟥.  
    - Evenly split ⚪.  
  - Stores all processed data in a database for further use.  
- **Technology**:  
  Written in **C#**.  

### 2. **PrepareGeoJson** 🌍  
This module converts the processed data into geographical formats.  
- **Purpose**:  
  Uses the data from the database to generate GeoJSON files for visualization.  
- **Features**:  
  - Creates two GeoJSON files:  
    - **District Courts GeoJSON**: Contains data for each district court.  
    - **Circuit Courts GeoJSON**: Contains data for each circuit court.  
  - Each GeoJSON is a `FeatureCollection` where:  
    - Each feature represents a court (district or circuit).  
    - Features include metadata, such as:  
      - Partisanship of the court.  
      - Number of vacancies.  
      - Number of active judges.  
    - Includes geometric points defining court jurisdictions.  
- **Technology**:  
  Written in **C#**.  

### 3. **DisplayMaps** 🖥️🗺️  
This module visualizes the data on an interactive map.  
- **Purpose**:  
  Presents court data through a web-based map using the GeoJSON files.  
- **Features**:  
  - Interactive map visualization using Leaflet.js.  
  - Separate layers for district courts and circuit courts.  
  - Displays metadata for each court when clicked.  
- **Technology**:  
  Written in **JavaScript**, **HTML**, and **CSS**.  

---  

## Geometric Data Source 🌐📍  

The geometric points defining the jurisdictions of the courts were sourced from publicly available datasets, including:  
- **US Court of Appeals Circuits Shapefile**:  
  Used for mapping circuit court boundaries. [Link](https://hub.arcgis.com/datasets/geoplatform::us-court-of-appeals-circuits-1/about)
- **US District Courts Shapefile**:  
  Used for mapping district court boundaries. [Link](https://www.arcgis.com/home/item.html?id=ed086b05d8dc46eab574e0fecdcf0f1f)
- **OpenStreetMap**:  
  Supplementary data to refine court jurisdiction boundaries.  

These datasets were processed to align with court jurisdictions and formatted into GeoJSON files for visualization.  

---  

## How to Run 🚀  

### Prerequisites 📋  
- **C# Runtime**: Required for `PrepareData` and `PrepareGeoJson` directories.  
- **Jekyll**: For running the `DisplayMaps` module locally.  
- **Leaflet.js**: Included as a dependency for interactive map rendering.  

### Steps  
1. **Prepare Data**:  
   Run the C# scripts in the `PrepareData` directory to scrape and process data into the database.  
2. **Generate GeoJSON**:  
   Execute the C# scripts in the `PrepareGeoJson` directory to create the GeoJSON files.  
3. **Display the Map**:  
   - Navigate to the `DisplayMaps` directory.  
   - Start the Jekyll server:  
     ```bash  
     bundle install
     bundle exec jekyll -v  
     ```  
   - Open the generated URL in your browser to view the map.  

---  

## Contributing  

Contributions are welcome! Feel free to submit issues or pull requests for bug fixes or enhancements.  

---  

## License 📄  

This project is licensed under the [MIT License](LICENSE).  
