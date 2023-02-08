UPDATE [Transcript] SET 
	[Name] = @Name, 
	[MediaFileSize] = @MediaFileSize, 
	[Status] = @Status,
	[Step] = @Step,
	[CaseNumber] = @CaseNumber,
	[JudgeName] = @JudgeName,
	[JudgeTitle] = @JudgeTitle,
	[TranscriptType] = @TranscriptType,
	[DeptNumber] = @DeptNumber,
	[TranscriptDateTime] = @RecordDate,
	[ApiLanguageId] = @ApiLanguageId,
	[HumanTranscriptionStatus] = @HumanTranscriptionStatus,
	[LastModifiedBy] = @LastModifiedBy,
	[LastModifiedOn] = GETUTCDATE()
WHERE Id = @Id