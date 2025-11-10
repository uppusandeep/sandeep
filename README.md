# Batchproducer Service

## Configuration Loading Fix

The Batchproducer service has been updated to properly handle configuration file loading with the following improvements:

### Changes Made

1. **Multiple Path Resolution**: The service now checks multiple standard locations for the configuration file:
   - Current working directory
   - User's home directory
   - System configuration directory (`/etc/batchproducer/`)
   - Classpath resources

2. **Better Error Handling**: 
   - Clear error messages indicating which paths were checked
   - Proper exception handling with informative messages
   - File existence and readability checks before attempting to load

3. **Fallback Mechanism**: 
   - Tries file system paths first
   - Falls back to classpath resources if file system paths fail
   - Provides detailed error message if all attempts fail

### Configuration File

The default configuration file is `batchproducer.properties` and should be placed in one of the following locations:

1. Current working directory: `./batchproducer.properties`
2. User home: `~/batchproducer.properties`
3. System config: `/etc/batchproducer/batchproducer.properties`
4. Classpath: Included in JAR as a resource

### Usage

```java
// Initialize the service
Batchproducer service = new Batchproducer();

// Access configuration
String batchSize = service.getProperty("batch.size");
String logLevel = service.getProperty("log.level", "INFO");
```

### Troubleshooting

If you encounter "failed to load configuration file" error:

1. Ensure `batchproducer.properties` exists in one of the standard locations
2. Check file permissions (must be readable)
3. Verify the file path is correct
4. Check the error message for details on which paths were attempted
