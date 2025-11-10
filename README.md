# Batchproducer Service

## Configuration Loading Fix

The Batchproducer service has been updated to properly handle configuration file loading with the following improvements:

### Changes Made

1. **Multiple Path Resolution**: The service now checks several locations for the configuration file, in priority order:
    - `batchproducer.config` JVM system property (e.g. `-Dbatchproducer.config=/opt/app/conf/batchproducer.properties`)
    - `BATCHPRODUCER_CONFIG` environment variable
    - Directory containing the running JAR (and its `config/` subdirectory)
    - Current working directory
    - User's home directory
    - System configuration directory (`/etc/batchproducer/`)
    - Classpath resources (e.g. bundled inside the JAR)

2. **Better Error Handling**: 
   - Clear error messages indicating which paths were checked
   - Proper exception handling with informative messages
   - File existence and readability checks before attempting to load

3. **Fallback Mechanism**: 
   - Tries file system paths first
   - Falls back to classpath resources if file system paths fail
   - Provides detailed error message if all attempts fail

### Configuration File

The default configuration file is `batchproducer.properties` and can be provided through any of the following mechanisms:

1. Pass a JVM system property: `-Dbatchproducer.config=/path/to/batchproducer.properties`
2. Set the `BATCHPRODUCER_CONFIG` environment variable
3. Place it alongside the deployed JAR (optionally inside a `config/` subdirectory)
4. Current working directory: `./batchproducer.properties`
5. User home: `~/batchproducer.properties`
6. System config: `/etc/batchproducer/batchproducer.properties`
7. Classpath: Include the file in the packaged JAR resources

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

1. Ensure `batchproducer.properties` exists in one of the supported locations or override mechanisms
2. Check file permissions (must be readable by the service user)
3. Verify the `batchproducer.config` system property or `BATCHPRODUCER_CONFIG` environment variable points to the correct file (if used)
4. Review the detailed error output to see every path that was attempted
