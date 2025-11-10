package com.example.service;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/**
 * BatchProducer Service - Handles batch processing operations
 * Loads configuration from external files
 */
public class BatchProducer {
    
    private Properties config;
    private static final String DEFAULT_CONFIG_PATH = "config/batchproducer.properties";
    private static final String RESOURCE_CONFIG_PATH = "/batchproducer.properties";
    
    public BatchProducer() {
        this.config = new Properties();
        loadConfiguration();
    }
    
    /**
     * Loads configuration from file with proper error handling
     * Tries multiple locations in order of preference:
     * 1. External config directory (config/batchproducer.properties)
     * 2. Classpath resources (/batchproducer.properties)
     * 3. System properties fallback
     */
    private void loadConfiguration() {
        boolean configLoaded = false;
        
        // Try loading from external config directory first
        try (FileInputStream fis = new FileInputStream(DEFAULT_CONFIG_PATH)) {
            config.load(fis);
            System.out.println("Configuration loaded successfully from: " + DEFAULT_CONFIG_PATH);
            configLoaded = true;
        } catch (IOException e) {
            System.out.println("Could not load config from external file: " + e.getMessage());
        }
        
        // If external config not found, try loading from classpath
        if (!configLoaded) {
            try (InputStream is = getClass().getResourceAsStream(RESOURCE_CONFIG_PATH)) {
                if (is != null) {
                    config.load(is);
                    System.out.println("Configuration loaded successfully from classpath: " + RESOURCE_CONFIG_PATH);
                    configLoaded = true;
                } else {
                    System.out.println("Configuration file not found in classpath: " + RESOURCE_CONFIG_PATH);
                }
            } catch (IOException e) {
                System.out.println("Error loading config from classpath: " + e.getMessage());
            }
        }
        
        // If no config loaded, use default values
        if (!configLoaded) {
            System.out.println("WARNING: No configuration file found. Using default values.");
            setDefaultConfiguration();
        }
        
        // Log loaded configuration
        logConfiguration();
    }
    
    /**
     * Sets default configuration values
     */
    private void setDefaultConfiguration() {
        config.setProperty("batch.size", "100");
        config.setProperty("batch.timeout", "30000");
        config.setProperty("batch.threads", "4");
        config.setProperty("output.directory", "./output");
        config.setProperty("error.retry.count", "3");
    }
    
    /**
     * Logs the current configuration (without sensitive data)
     */
    private void logConfiguration() {
        System.out.println("=== BatchProducer Configuration ===");
        config.forEach((key, value) -> {
            // Don't log sensitive keys like passwords
            if (!key.toString().toLowerCase().contains("password") && 
                !key.toString().toLowerCase().contains("secret")) {
                System.out.println(key + " = " + value);
            }
        });
        System.out.println("===================================");
    }
    
    /**
     * Gets a configuration property
     * @param key The property key
     * @return The property value, or null if not found
     */
    public String getProperty(String key) {
        return config.getProperty(key);
    }
    
    /**
     * Gets a configuration property with a default value
     * @param key The property key
     * @param defaultValue The default value if key not found
     * @return The property value or default value
     */
    public String getProperty(String key, String defaultValue) {
        return config.getProperty(key, defaultValue);
    }
    
    /**
     * Gets an integer configuration property
     * @param key The property key
     * @param defaultValue The default value if key not found or invalid
     * @return The integer property value
     */
    public int getIntProperty(String key, int defaultValue) {
        String value = config.getProperty(key);
        if (value != null) {
            try {
                return Integer.parseInt(value.trim());
            } catch (NumberFormatException e) {
                System.err.println("Invalid integer value for " + key + ": " + value);
            }
        }
        return defaultValue;
    }
    
    /**
     * Process a batch - example method
     */
    public void processBatch() {
        int batchSize = getIntProperty("batch.size", 100);
        int timeout = getIntProperty("batch.timeout", 30000);
        String outputDir = getProperty("output.directory", "./output");
        
        System.out.println("Processing batch with:");
        System.out.println("  Batch size: " + batchSize);
        System.out.println("  Timeout: " + timeout + "ms");
        System.out.println("  Output directory: " + outputDir);
        
        // Add your batch processing logic here
    }
    
    /**
     * Main method for testing
     */
    public static void main(String[] args) {
        try {
            BatchProducer producer = new BatchProducer();
            producer.processBatch();
        } catch (Exception e) {
            System.err.println("Error starting BatchProducer: " + e.getMessage());
            e.printStackTrace();
        }
    }
}
