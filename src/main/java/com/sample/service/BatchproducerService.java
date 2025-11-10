package com.sample.service;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;
import java.util.logging.Logger;
import java.util.logging.Level;

/**
 * BatchproducerService - Service for batch processing operations
 * Handles configuration file loading with proper error handling
 */
public class BatchproducerService {
    private static final Logger logger = Logger.getLogger(BatchproducerService.class.getName());
    private static final String DEFAULT_CONFIG_FILE = "batchproducer.properties";
    private Properties configuration;
    
    public BatchproducerService() {
        this.configuration = new Properties();
    }
    
    /**
     * Loads configuration from the default configuration file
     * @return true if configuration loaded successfully, false otherwise
     */
    public boolean loadConfiguration() {
        return loadConfiguration(DEFAULT_CONFIG_FILE);
    }
    
    /**
     * Loads configuration from a specified file path
     * @param configFilePath Path to the configuration file
     * @return true if configuration loaded successfully, false otherwise
     */
    public boolean loadConfiguration(String configFilePath) {
        if (configFilePath == null || configFilePath.trim().isEmpty()) {
            logger.severe("Configuration file path is null or empty");
            return false;
        }
        
        // Try multiple loading strategies
        // 1. Try as resource from classpath
        if (loadFromClasspath(configFilePath)) {
            logger.info("Configuration loaded successfully from classpath: " + configFilePath);
            return true;
        }
        
        // 2. Try as absolute file path
        if (loadFromAbsolutePath(configFilePath)) {
            logger.info("Configuration loaded successfully from absolute path: " + configFilePath);
            return true;
        }
        
        // 3. Try as relative file path
        if (loadFromRelativePath(configFilePath)) {
            logger.info("Configuration loaded successfully from relative path: " + configFilePath);
            return true;
        }
        
        // 4. Try in common configuration directories
        if (loadFromCommonPaths(configFilePath)) {
            logger.info("Configuration loaded successfully from common path: " + configFilePath);
            return true;
        }
        
        logger.severe("Failed to load configuration file from any location: " + configFilePath);
        return false;
    }
    
    /**
     * Attempts to load configuration from classpath
     */
    private boolean loadFromClasspath(String configFilePath) {
        try (InputStream inputStream = getClass().getClassLoader().getResourceAsStream(configFilePath)) {
            if (inputStream != null) {
                configuration.load(inputStream);
                return true;
            }
        } catch (IOException e) {
            logger.log(Level.WARNING, "Failed to load configuration from classpath: " + configFilePath, e);
        }
        return false;
    }
    
    /**
     * Attempts to load configuration from absolute file path
     */
    private boolean loadFromAbsolutePath(String configFilePath) {
        File configFile = new File(configFilePath);
        if (configFile.isAbsolute() && configFile.exists() && configFile.isFile() && configFile.canRead()) {
            try (FileInputStream fis = new FileInputStream(configFile)) {
                configuration.load(fis);
                return true;
            } catch (IOException e) {
                logger.log(Level.WARNING, "Failed to load configuration from absolute path: " + configFilePath, e);
            }
        }
        return false;
    }
    
    /**
     * Attempts to load configuration from relative file path
     */
    private boolean loadFromRelativePath(String configFilePath) {
        File configFile = new File(configFilePath);
        if (!configFile.isAbsolute() && configFile.exists() && configFile.isFile() && configFile.canRead()) {
            try (FileInputStream fis = new FileInputStream(configFile)) {
                configuration.load(fis);
                return true;
            } catch (IOException e) {
                logger.log(Level.WARNING, "Failed to load configuration from relative path: " + configFilePath, e);
            }
        }
        return false;
    }
    
    /**
     * Attempts to load configuration from common configuration directories
     */
    private boolean loadFromCommonPaths(String configFilePath) {
        String[] commonPaths = {
            System.getProperty("user.home") + "/.config/" + configFilePath,
            System.getProperty("user.dir") + "/config/" + configFilePath,
            "/etc/" + configFilePath,
            System.getProperty("user.dir") + "/" + configFilePath
        };
        
        for (String path : commonPaths) {
            File configFile = new File(path);
            if (configFile.exists() && configFile.isFile() && configFile.canRead()) {
                try (FileInputStream fis = new FileInputStream(configFile)) {
                    configuration.load(fis);
                    return true;
                } catch (IOException e) {
                    logger.log(Level.FINE, "Failed to load from common path: " + path, e);
                }
            }
        }
        return false;
    }
    
    /**
     * Gets a configuration property value
     * @param key The property key
     * @return The property value, or null if not found
     */
    public String getProperty(String key) {
        return configuration.getProperty(key);
    }
    
    /**
     * Gets a configuration property value with a default
     * @param key The property key
     * @param defaultValue Default value if key not found
     * @return The property value or default
     */
    public String getProperty(String key, String defaultValue) {
        return configuration.getProperty(key, defaultValue);
    }
    
    /**
     * Gets all configuration properties
     * @return Properties object containing all configuration
     */
    public Properties getConfiguration() {
        return new Properties(configuration);
    }
    
    /**
     * Checks if configuration is loaded
     * @return true if configuration is loaded, false otherwise
     */
    public boolean isConfigurationLoaded() {
        return configuration != null && !configuration.isEmpty();
    }
}
