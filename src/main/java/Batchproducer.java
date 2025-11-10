import java.io.*;
import java.nio.file.*;
import java.util.Properties;

/**
 * Batchproducer Service
 * Handles batch processing with configuration file loading
 */
public class Batchproducer {
    private static final String CONFIG_FILE_NAME = "batchproducer.properties";
    private Properties config;
    
    public Batchproducer() {
        loadConfiguration();
    }
    
    /**
     * Loads configuration file with proper error handling and path resolution
     */
    private void loadConfiguration() {
        config = new Properties();
        
        // Try multiple locations for the configuration file
        String[] possiblePaths = {
            // 1. Current working directory
            CONFIG_FILE_NAME,
            // 2. User's home directory
            System.getProperty("user.home") + File.separator + CONFIG_FILE_NAME,
            // 3. System configuration directory
            "/etc/batchproducer/" + CONFIG_FILE_NAME,
            // 4. Classpath resource
            null // Will be handled separately
        };
        
        boolean loaded = false;
        String loadedPath = null;
        
        // Try loading from file system paths
        for (String path : possiblePaths) {
            if (path == null) continue;
            
            try {
                File configFile = new File(path);
                if (configFile.exists() && configFile.isFile() && configFile.canRead()) {
                    try (FileInputStream fis = new FileInputStream(configFile)) {
                        config.load(fis);
                        loaded = true;
                        loadedPath = path;
                        System.out.println("Configuration loaded from: " + configFile.getAbsolutePath());
                        break;
                    }
                }
            } catch (IOException e) {
                // Continue to next path
                System.err.println("Failed to load from " + path + ": " + e.getMessage());
            }
        }
        
        // Try loading from classpath as fallback
        if (!loaded) {
            try (InputStream is = getClass().getClassLoader().getResourceAsStream(CONFIG_FILE_NAME)) {
                if (is != null) {
                    config.load(is);
                    loaded = true;
                    loadedPath = "classpath:" + CONFIG_FILE_NAME;
                    System.out.println("Configuration loaded from classpath: " + CONFIG_FILE_NAME);
                }
            } catch (IOException e) {
                System.err.println("Failed to load from classpath: " + e.getMessage());
            }
        }
        
        // If still not loaded, create default configuration or throw exception
        if (!loaded) {
            String errorMsg = String.format(
                "Failed to load configuration file '%s' from any of the following locations:\n" +
                "  - Current directory: %s\n" +
                "  - User home: %s\n" +
                "  - System config: /etc/batchproducer/%s\n" +
                "  - Classpath: %s\n" +
                "Please ensure the configuration file exists in one of these locations.",
                CONFIG_FILE_NAME,
                new File(CONFIG_FILE_NAME).getAbsolutePath(),
                System.getProperty("user.home") + File.separator + CONFIG_FILE_NAME,
                CONFIG_FILE_NAME,
                CONFIG_FILE_NAME
            );
            
            System.err.println(errorMsg);
            throw new RuntimeException("Failed to load configuration file: " + CONFIG_FILE_NAME, 
                new FileNotFoundException("Configuration file not found in any standard location"));
        }
    }
    
    /**
     * Gets a configuration property value
     */
    public String getProperty(String key) {
        return config.getProperty(key);
    }
    
    /**
     * Gets a configuration property value with default
     */
    public String getProperty(String key, String defaultValue) {
        return config.getProperty(key, defaultValue);
    }
    
    /**
     * Gets all configuration properties
     */
    public Properties getConfig() {
        return new Properties(config); // Return a copy for safety
    }
    
    /**
     * Main method for testing
     */
    public static void main(String[] args) {
        try {
            Batchproducer service = new Batchproducer();
            System.out.println("Batchproducer service initialized successfully");
            System.out.println("Configuration properties: " + service.getConfig().size());
        } catch (Exception e) {
            System.err.println("Error initializing Batchproducer service: " + e.getMessage());
            e.printStackTrace();
            System.exit(1);
        }
    }
}
