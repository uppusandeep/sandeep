import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.net.URISyntaxException;
import java.net.URL;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Properties;
import java.util.Set;

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
        Set<Path> candidatePaths = new LinkedHashSet<>();
        List<String> attemptedLocations = new ArrayList<>();

        // Highest priority: explicit overrides
        String systemPropertyPath = System.getProperty("batchproducer.config");
        if (systemPropertyPath != null && !systemPropertyPath.isBlank()) {
            candidatePaths.add(Paths.get(systemPropertyPath.trim()));
        }

        String envPath = System.getenv("BATCHPRODUCER_CONFIG");
        if (envPath != null && !envPath.isBlank()) {
            candidatePaths.add(Paths.get(envPath.trim()));
        }

        // Application locations
        Path applicationDirectory = resolveApplicationDirectory();
        if (applicationDirectory != null) {
            candidatePaths.add(applicationDirectory.resolve(CONFIG_FILE_NAME));
            candidatePaths.add(applicationDirectory.resolve("config").resolve(CONFIG_FILE_NAME));
        }

        // Standard locations
        candidatePaths.add(Paths.get(CONFIG_FILE_NAME)); // Current working directory
        candidatePaths.add(Paths.get(System.getProperty("user.home"), CONFIG_FILE_NAME)); // User home directory
        candidatePaths.add(Paths.get("/etc/batchproducer", CONFIG_FILE_NAME)); // System configuration directory

        for (Path candidate : candidatePaths) {
            Path normalized = candidate.toAbsolutePath().normalize();
            attemptedLocations.add(normalized.toString());

            if (!Files.exists(normalized)) {
                continue;
            }
            if (!Files.isRegularFile(normalized) || !Files.isReadable(normalized)) {
                System.err.println("Configuration file found but not readable: " + normalized);
                continue;
            }

            try (InputStream inputStream = Files.newInputStream(normalized)) {
                config.load(inputStream);
                System.out.println("Configuration loaded from: " + normalized);
                return;
            } catch (IOException e) {
                System.err.println("Failed to load configuration from " + normalized + ": " + e.getMessage());
            }
        }

        // Try loading from classpath as fallback
        ClassLoader classLoader = Thread.currentThread().getContextClassLoader();
        if (classLoader == null) {
            classLoader = getClass().getClassLoader();
        }

        String[] classpathCandidates = {
            CONFIG_FILE_NAME,
            "config/" + CONFIG_FILE_NAME
        };

        for (String resourceName : classpathCandidates) {
            attemptedLocations.add("classpath:" + resourceName);
            try (InputStream is = classLoader.getResourceAsStream(resourceName)) {
                if (is != null) {
                    config.load(is);
                    System.out.println("Configuration loaded from classpath: " + resourceName);
                    return;
                }
            } catch (IOException e) {
                System.err.println("Failed to load configuration from classpath resource " + resourceName + ": " + e.getMessage());
            }
        }

        // If still not loaded, throw exception with detailed message
        StringBuilder errorMsg = new StringBuilder();
        errorMsg.append("Failed to load configuration file '")
                .append(CONFIG_FILE_NAME)
                .append("' from any known location.\nAttempted locations:");
        for (String location : attemptedLocations) {
            errorMsg.append("\n  - ").append(location);
        }
        errorMsg.append("\nPlease set the 'batchproducer.config' system property, the 'BATCHPRODUCER_CONFIG' environment variable, or place the file in one of the attempted locations.");

        System.err.println(errorMsg.toString());
        throw new IllegalStateException("Failed to load configuration file: " + CONFIG_FILE_NAME,
            new FileNotFoundException("Configuration file not found in any tried location"));
    }
    
    /**
     * Determines the directory where the application (JAR or classes) is located.
     */
    private Path resolveApplicationDirectory() {
        try {
            URL location = getClass().getProtectionDomain().getCodeSource().getLocation();
            if (location == null) {
                return null;
            }

            Path path = Paths.get(location.toURI());
            if (Files.isRegularFile(path)) {
                return path.getParent();
            }
            if (Files.isDirectory(path)) {
                return path;
            }
        } catch (URISyntaxException e) {
            System.err.println("Unable to resolve application directory: " + e.getMessage());
        }
        return null;
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
        Properties copy = new Properties();
        copy.putAll(config);
        return copy;
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
