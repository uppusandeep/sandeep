package com.sample;

import com.sample.service.BatchproducerService;
import java.util.logging.Logger;

/**
 * Main class to demonstrate BatchproducerService usage
 */
public class Main {
    private static final Logger logger = Logger.getLogger(Main.class.getName());
    
    public static void main(String[] args) {
        BatchproducerService service = new BatchproducerService();
        
        // Try to load configuration
        String configFile = args.length > 0 ? args[0] : null;
        
        boolean loaded;
        if (configFile != null) {
            logger.info("Attempting to load configuration from: " + configFile);
            loaded = service.loadConfiguration(configFile);
        } else {
            logger.info("Attempting to load default configuration");
            loaded = service.loadConfiguration();
        }
        
        if (loaded) {
            logger.info("Configuration loaded successfully!");
            logger.info("Batch size: " + service.getProperty("batch.size", "not set"));
            logger.info("Service enabled: " + service.getProperty("service.enabled", "not set"));
        } else {
            logger.severe("Failed to load configuration file!");
            System.exit(1);
        }
    }
}
