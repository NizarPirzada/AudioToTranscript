INSERT INTO [dbo].[Transcript_Dialog] (
	[TranscriptId], 
	[PersonId], 
	[StartTime], 
	[Duration], 
	[Transcription], 
	[OriginalSpeakerId],
	[OriginalSpeakerName],
	[ExaminationType],
	[ExaminationTag]) 
VALUES (
	@TranscriptId, 
	NULL, 
	@StartTime, 
	@Duration, 
	@Transcription, 
	@OriginalSpeakerId,
	@OriginalSpeakerName,
	@ExaminationType,
	@ExaminationTag);
SELECT CAST(SCOPE_IDENTITY() as int)