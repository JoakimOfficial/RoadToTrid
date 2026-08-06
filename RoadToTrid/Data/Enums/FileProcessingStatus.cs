namespace RoadToTrid.Data.Enums;

public enum FileProcessingStatus
{
    Waiting,       // The file is queued, waiting to be processed.
    Processing,    // The file is currently being processed.
    Done,          // The file has been successfully processed.
    Failed         // Processing failed for this file.
}
