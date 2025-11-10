# BatchProducer Service

## Overview
This project contains a BatchProducer service that processes batch operations with configurable settings.

## Configuration

### Configuration File Loading Order
The BatchProducer service attempts to load configuration in the following order:

1. **External Config Directory**: `config/batchproducer.properties` (highest priority)
2. **Classpath Resources**: `/batchproducer.properties` (fallback)
3. **Default Values**: Hard-coded defaults (last resort)

### Configuration File Location
Place your `batchproducer.properties` file in one of these locations:

- **Recommended**: `config/batchproducer.properties` (external to JAR)
- **Alternative**: `src/main/resources/batchproducer.properties` (packaged in JAR)

### Configuration Properties

| Property | Description | Default Value |
|----------|-------------|---------------|
| `batch.size` | Number of items to process per batch | 100 |
| `batch.timeout` | Timeout for batch processing (ms) | 30000 |
| `batch.threads` | Number of processing threads | 4 |
| `output.directory` | Directory for output files | ./output |
| `output.format` | Output file format | csv |
| `error.retry.count` | Number of retries on error | 3 |
| `error.retry.delay` | Delay between retries (ms) | 5000 |
| `log.level` | Logging level | INFO |
| `log.file` | Log file location | ./logs/batchproducer.log |

## Building the Project

```bash
# Compile the project
javac -d bin src/main/java/com/example/service/BatchProducer.java

# Run the BatchProducer
java -cp bin com.example.service.BatchProducer
```

## Troubleshooting

### "Failed to load configuration file"

If you see this error, check:

1. **File exists**: Verify `config/batchproducer.properties` exists
2. **File permissions**: Ensure the file is readable
3. **File path**: Check the working directory is correct
4. **File format**: Verify properties file format is correct (key=value)

The service will still run with default values if no configuration file is found.

### Configuration Not Loading

1. Check file location relative to execution directory
2. Verify file name matches exactly: `batchproducer.properties`
3. Check for any syntax errors in the properties file
4. Review console output for configuration loading messages

## Project Structure

```
/workspace/
├── config/
│   └── batchproducer.properties    # External configuration file
├── src/
│   └── main/
│       ├── java/
│       │   └── com/
│       │       └── example/
│       │           └── service/
│       │               └── BatchProducer.java
│       └── resources/
│           └── batchproducer.properties    # Classpath configuration
└── README.md
```

## Usage Example

```java
// Create a new BatchProducer instance
BatchProducer producer = new BatchProducer();

// Configuration is automatically loaded
// Process batches
producer.processBatch();

// Access configuration values
String outputDir = producer.getProperty("output.directory");
int batchSize = producer.getIntProperty("batch.size", 100);
```

## Fix Applied

This implementation fixes the "failed to load configuration file" error by:

1. **Multiple fallback locations**: Tries external config, then classpath
2. **Proper error handling**: Catches IOException and provides clear error messages
3. **Default values**: Falls back to sensible defaults if no config found
4. **Detailed logging**: Shows where configuration was loaded from
5. **Resource management**: Uses try-with-resources for proper file handling
