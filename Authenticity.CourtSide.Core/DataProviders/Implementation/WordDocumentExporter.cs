using Authenticity.CourtSide.Core.Enums;
using Authenticity.CourtSide.Core.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using ParagraphProperties = DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace Authenticity.CourtSide.Core.DataProviders.Implementation
{
	public class WordDocumentExporter : IFileExporter
	{
		private const string US_CULTURE_INFO = "en-US";
		private const string ERROR_TRANSCRIPT_TYPE_NOT_FOUND = "Transcript type not found";
		private const string ERROR_TRANSCRIPT_TAG_NOT_FOUND = "Transcript tag not found";

		private readonly ILogger<WordDocumentExporter> _logger;
		public WordDocumentExporter(ILogger<WordDocumentExporter> logger)
		{
			_logger = logger;
		}

		public string CreateExportDocument(string pathFile, Transcript transcript, IEnumerable<TranscriptPerson> persons, IEnumerable<TranscriptDialog> formatedDialog, int offset)
		{
			string documentPath = string.Empty;
			DateTime? recordDateOffset = null;
			TextInfo textInfo = new CultureInfo(US_CULTURE_INFO, false).TextInfo;

			if (transcript.RecordDate.HasValue)
			{
				recordDateOffset = transcript.RecordDate.Value.AddMinutes(offset);
			}

			if (File.Exists(pathFile))
			{
				documentPath = pathFile.Replace(".docx", "_export.docx");
				if (File.Exists(documentPath))
				{
					File.Delete(documentPath);
				}
				try
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						byte[] byteArray = File.ReadAllBytes(pathFile);
						memoryStream.Write(byteArray, 0, (int)byteArray.Length);
						using (var wordDocument = WordprocessingDocument.Open(memoryStream, true))
						{
							Body documentBody = wordDocument.MainDocumentPart.Document.Body;
							IEnumerable<HeaderPart> documentHeaders = wordDocument.MainDocumentPart.HeaderParts;
							Table introTable = wordDocument.MainDocumentPart.Document.Body.Elements<Table>().FirstOrDefault();

							// Probable Cause Hearing fields
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TCITY", GetStringDefaultValue(transcript.Court?.City, "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TSTATE", GetStringDefaultValue(transcript.Court?.State?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TDATE", GetLongDateFormatted(recordDateOffset, "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "THOUR", GetTimeFormatted(recordDateOffset, "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TCOURT", GetStringDefaultValue(transcript.Court?.Name?.ToUpper(), "____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TPLAINTIFFNAME", GetStringDefaultValue(persons.Where(x => x.Type == PersonType.Plaintiff).FirstOrDefault()?.FullName?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TDEFENDANTNAME", GetStringDefaultValue(persons.Where(x => x.Type == PersonType.Defendant).FirstOrDefault()?.FullName?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TCASENUMBER", GetStringDefaultValue(transcript.CaseNumber?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TJUDGENAME", GetStringDefaultValue(transcript.JudgeName?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TJUDGETITLE", GetStringDefaultValue(transcript.JudgeTitle?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TCOURT", GetStringDefaultValue(transcript.Court?.Name?.ToUpper(), "____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TCOUNTY", GetStringDefaultValue(transcript.Court.City?.ToUpper(), "_____"));

							// Deposition Fields

							if (introTable.HasChildren)
							{
								FillTableCell(introTable, "TPLAINTIFFNAME", GetStringDefaultValue(persons.Where(x => x.Type == Enums.PersonType.Plaintiff).FirstOrDefault()?.FullName?.ToUpper(), "_____"));
								FillTableCell(introTable, "TCASENUMBER", GetStringDefaultValue(transcript.CaseNumber?.ToUpper(), "____"));
								FillTableCell(introTable, "TDEPTNUMBER", GetStringDefaultValue(transcript.DeptNumber?.ToUpper(), "_____"));
								FillTableCell(introTable, "TDEFENDANTNAME", GetStringDefaultValue(persons.Where(x => x.Type == Enums.PersonType.Defendant).FirstOrDefault()?.FullName?.ToUpper(), "_____"));
							}

							ReplaceAllTextOccurrencesOnDocument(documentBody, "TESDATE", GetShortDateFormatted(recordDateOffset, "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TDEPONENTFULLNAME", GetStringDefaultValue(GetDeponentFullName(persons).ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TDEPONENTNAME", GetStringDefaultValue(persons.Where(x => x.Type == Enums.PersonType.Deponent).FirstOrDefault()?.FullName?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TRIVENUECOMPANY", GetStringDefaultValue(transcript.TranscriptRecordingInfo.VenueCompany?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TRIDATE", GetCustomDateFormatted(recordDateOffset, "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TRISTREETADDRESS", GetStringDefaultValue(transcript.TranscriptRecordingInfo.StreetAddress?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TRICITY", GetStringDefaultValue(transcript.TranscriptRecordingInfo.City?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TRISTATE", GetStringDefaultValue(transcript.TranscriptRecordingInfo.State?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TRIZIPCODE", GetStringDefaultValue(transcript.TranscriptRecordingInfo.Zipcode?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TDEPONENTLASTNAME", GetStringDefaultValue(persons.Where(x => x.Type == Enums.PersonType.Deponent).FirstOrDefault()?.LastName?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TDEPONENTFIRSTNAME", GetStringDefaultValue(persons.Where(x => x.Type == Enums.PersonType.Deponent).FirstOrDefault()?.FirstName?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TPLAINTIFFVSTDEFENDANT", GetStringDefaultValue(GetDeponentVersusField(persons), "_____ vs _____"));
							ReplaceAllTextOccurrencesOnDocument(documentBody, "TDEPONENTNAMELOWER", textInfo.ToTitleCase(GetStringDefaultValue(persons.Where(x => x.Type == Enums.PersonType.Deponent).FirstOrDefault()?.FullName?.ToLower(), "_____")));

							// Deposition Headers
							ReplaceAllTextOccurrencesOnDocumentHeader(documentHeaders, "TPLAINTIFFVSTDEFENDANT", GetStringDefaultValue(GetDeponentVersusField(persons), "_____ vs _____"));
							ReplaceAllTextOccurrencesOnDocumentHeader(documentHeaders, "TDEPONENTLASTNAME", GetStringDefaultValue(persons.Where(x => x.Type == Enums.PersonType.Deponent).FirstOrDefault()?.LastName?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocumentHeader(documentHeaders, "TDEPONENTFIRSTNAME", GetStringDefaultValue(persons.Where(x => x.Type == Enums.PersonType.Deponent).FirstOrDefault()?.FirstName?.ToUpper(), "_____"));
							ReplaceAllTextOccurrencesOnDocumentHeader(documentHeaders, "TDATE", GetShortDateFormatted(recordDateOffset, "_____"));

							// Attorneys Information
							FillAttorneyInformation(documentBody, persons.Where(x => x.Type == PersonType.PlaintiffAttorney), PersonType.PlaintiffAttorney, transcript.TranscriptType);
							FillAttorneyInformation(documentBody, persons.Where(x => x.Type == PersonType.DefendantAttorney), PersonType.DefendantAttorney, transcript.TranscriptType);

							// Transcription paragraphs
							FillTranscriptionParagraphs(documentBody, formatedDialog, transcript.TranscriptType);

						}
						using (FileStream fs = new FileStream(documentPath, FileMode.Create))
						{
							memoryStream.WriteTo(fs);
						}
					}
				}
				catch (Exception ex)
				{
					_logger.LogError($"Word document export failed: {ex.Message}", ex);
				}
			}
			return documentPath;
		}

		private string GetStringDefaultValue(string originalValue, string defaultValue = "")
		{
			return originalValue != null ? originalValue : defaultValue;
		}

		private string GetLongDateFormatted(DateTime? originalDate, string defaultValue = "")
		{
			return originalDate.HasValue ? originalDate.Value.ToLongDateString() : defaultValue;
		}

		private string GetCustomDateFormatted(DateTime? originalDate, string defaultValue = "")
		{
			return originalDate.HasValue ? originalDate.Value.ToString("MMMM dd, yyyy") : defaultValue;
		}

		private string GetShortDateFormatted(DateTime? originalDate, string defaultValue = "")
		{
			return originalDate.HasValue ? originalDate.Value.ToShortDateString() : defaultValue;
		}

		private string GetTimeFormatted(DateTime? originalDate, string defaultValue = "")
		{
			return originalDate.HasValue ? originalDate.Value.ToShortTimeString() : defaultValue;
		}

		private Paragraph GetParagraphFromTranscriptionSegment(string transcriptionSegment)
		{
			Paragraph paragraph = new Paragraph();
			var paragraphProperties = new ParagraphProperties();
			var paragraphStyleId = new ParagraphStyleId() { Val = "Normal" };
			var spacingDoubleLine = new SpacingBetweenLines() { After = "0", Before = "0", Line = "480", LineRule = LineSpacingRuleValues.Auto };
			var indentation = new Indentation() { Hanging = "720", Left = "720" };
			paragraphProperties.Append(spacingDoubleLine);
			paragraphProperties.Append(indentation);
			paragraphProperties.Append(paragraphStyleId);
			var runProperties = new RunProperties();
			var fontSize = new FontSize() { Val = "24" };
			var runFonts = new RunFonts() { Ascii = "Courier New" };
			runProperties.Append(fontSize);
			runProperties.Append(runFonts);

			var run = new Run();
			var text = new Text(transcriptionSegment);

			run.Append(runProperties);
			run.Append(new TabChar());
			run.Append(new TabChar());
			run.Append(new TabChar());
			run.Append(text);
			paragraph.Append(paragraphProperties);
			paragraph.Append(run);
			return paragraph;
		}
		private Paragraph GetParagraphForExaminationTitle(string transcriptionSegment)
		{
			Paragraph paragraph = new Paragraph();
			var paragraphProperties = new ParagraphProperties();
			var paragraphStyleId = new ParagraphStyleId() { Val = "Normal" };
			var spacingDoubleLine = new SpacingBetweenLines() { After = "0", Before = "0", Line = "480", LineRule = LineSpacingRuleValues.Auto };
			var indentation = new Indentation() { Hanging = "720", Left = "720" };
			Justification centerHeading = new Justification() { Val = JustificationValues.Center };

			paragraphProperties.Append(spacingDoubleLine);
			paragraphProperties.Append(indentation);
			paragraphProperties.Append(paragraphStyleId);
			paragraphProperties.Append(centerHeading);

			var runProperties = new RunProperties();
			var fontSize = new FontSize() { Val = "24" };
			var runFonts = new RunFonts() { Ascii = "Courier New" };
			runProperties.Append(fontSize);
			runProperties.Append(runFonts);

			var run = new Run();
			var text = new Text(transcriptionSegment);

			run.Append(runProperties);
			run.Append(text);
			paragraph.Append(paragraphProperties);
			paragraph.Append(run);
			return paragraph;
		}
		private Paragraph GetParagraphForExamination(string transcriptionSegment)
		{
			Paragraph paragraph = new Paragraph();
			var paragraphProperties = new ParagraphProperties();
			var paragraphStyleId = new ParagraphStyleId() { Val = "Normal" };
			var spacingDoubleLine = new SpacingBetweenLines() { After = "0", Before = "0", Line = "480", LineRule = LineSpacingRuleValues.Auto };
			var indentation = new Indentation() { Hanging = "720", Left = "720" };
			paragraphProperties.Append(spacingDoubleLine);
			paragraphProperties.Append(indentation);
			paragraphProperties.Append(paragraphStyleId);
			var runProperties = new RunProperties();
			var fontSize = new FontSize() { Val = "24" };
			var runFonts = new RunFonts() { Ascii = "Courier New" };
			runProperties.Append(fontSize);
			runProperties.Append(runFonts);

			var run = new Run();
			var text = new Text(transcriptionSegment);

			run.Append(runProperties);
			run.Append(text);
			paragraph.Append(paragraphProperties);
			paragraph.Append(run);
			return paragraph;
		}
		private Paragraph GetParagraphForDepositionSegment(string transcriptionSegment, bool isBoldSection = false)
		{
			Paragraph paragraph = new Paragraph();
			var paragraphProperties = new ParagraphProperties();
			var paragraphStyleId = new ParagraphStyleId() { Val = "Normal" };
			var spacingDoubleLine = new SpacingBetweenLines() { After = "0", Before = "0", Line = "480", LineRule = LineSpacingRuleValues.Auto };
			paragraphProperties.Append(spacingDoubleLine);
			paragraphProperties.Append(paragraphStyleId);
			var runProperties = new RunProperties();
			Bold bold = new Bold();
			bold.Val = OnOffValue.FromBoolean(isBoldSection);
			runProperties.AppendChild(bold);
			var fontSize = new FontSize() { Val = "24" };
			var runFonts = new RunFonts() { Ascii = "Courier New" };
			runProperties.Append(fontSize);
			runProperties.Append(runFonts);

			var run = new Run();
			var text = new Text(transcriptionSegment);

			run.Append(runProperties);
			run.Append(new TabChar());
			run.Append(new TabChar());
			run.Append(new TabChar());
			run.Append(text);
			paragraph.Append(paragraphProperties);
			paragraph.Append(run);
			return paragraph;
		}

		private void FillAttorneyInformation(Body documentBody, IEnumerable<TranscriptPerson> attorneys, PersonType type, TranscriptTypeEnum transcriptType)
		{
			// Probable cause hearing fields
			int amountAttorneys = default;
			string nameTemplate = type == PersonType.PlaintiffAttorney ? "TPLAINTIFFATTORNEYNAME" : "TDEFENDANTATTORNEYNAME";
			string barTemplate = type == PersonType.PlaintiffAttorney ? "TPLAINTIFFATTORNEYBAR" : "TDEFENDANTATTORNEYBAR";
			string tittleTemplate = type == PersonType.PlaintiffAttorney ? "TPLAINTIFFATTORNEYTITLE" : "TDEFENDANTATTORNEYTITLE";
			string addressTemplate = type == PersonType.PlaintiffAttorney ? "TPLAINTIFFATTORNEYADDRESS" : "TDEFENDANTATTORNEYADDRESS";
			string phoneTemplate = type == PersonType.PlaintiffAttorney ? "TPLAINTIFFATTORNEYPHONE" : "TDEFENDANTATTORNEYPHONE";
			Tuple<Paragraph, Run> newLineTemplate = GetRunText(documentBody, phoneTemplate);

			// Deposition fields
			TextInfo textInfo = new CultureInfo(US_CULTURE_INFO, false).TextInfo;
			int amountDepositionAttorneys = default;
			string nameDepositionTemplate = type == PersonType.PlaintiffAttorney ? "TPLAINTATTNAME" : "TDEFATTNAME";
			string tittleDepositionTemplate = type == PersonType.PlaintiffAttorney ? "TPLAINTATITLE" : "TDEFATTTITLE";
			string legalFirmDepositionTemplate = type == PersonType.PlaintiffAttorney ? "TPLAINTIFFATTORNEYLEGALFIRM" : "TDEFENDANTATTORNEYLEGALFIRM";
			string addressDepositionTemplate = type == PersonType.PlaintiffAttorney ? "TPLAINTIFFATTORNEYADDRESS" : "TDEFENDANTATTORNEYADDRESS";
			Tuple<Paragraph, Run> newLineDepositionTemplate = GetRunText(documentBody, addressDepositionTemplate);

			if (attorneys != null && attorneys.Any())
			{
				switch (transcriptType)
				{
					case TranscriptTypeEnum.ProbableCauseHearing:

						foreach (TranscriptPerson attorney in attorneys)
						{
							if (amountAttorneys == default)
							{
								ReplaceFirstTextOccurrenceOnDocument(documentBody, nameTemplate, (attorney.FullName?.ToUpper() ?? string.Empty));
								ReplaceFirstTextOccurrenceOnDocument(documentBody, tittleTemplate, (attorney.AdditionalInfo?.Title ?? string.Empty));
								ReplaceFirstTextOccurrenceOnDocument(documentBody, addressTemplate, (attorney.AdditionalInfo?.Address ?? string.Empty));
								ReplaceFirstTextOccurrenceOnDocument(documentBody, phoneTemplate, (attorney.AdditionalInfo?.Telephone ?? string.Empty));
								ReplaceFirstTextOccurrenceOnDocument(documentBody, barTemplate, (attorney.AdditionalInfo?.BarNumber ?? string.Empty));
							}
							else
							{
								Paragraph paragraph = newLineTemplate.Item1;
								paragraph.InsertAfterSelf(GetNewParagrahFromTemplate(paragraph, (attorney.AdditionalInfo?.Telephone ?? string.Empty)));
								paragraph.InsertAfterSelf(GetNewParagrahFromTemplate(paragraph, (attorney.AdditionalInfo?.Address ?? string.Empty)));
								paragraph.InsertAfterSelf(GetNewParagrahFromTemplate(paragraph, (attorney.AdditionalInfo?.Title ?? string.Empty)));
								paragraph.InsertAfterSelf(GetNewParagrahFromTemplate(paragraph, $"{(attorney.FullName.ToUpper() ?? string.Empty)} {(attorney.AdditionalInfo?.BarNumber ?? string.Empty)}"));
								paragraph.InsertAfterSelf(GetNewParagrahFromTemplate(paragraph, string.Empty));
							}
							amountAttorneys++;
						}
						break;

					case TranscriptTypeEnum.Deposition:

						foreach (TranscriptPerson attorney in attorneys)
						{

							if (amountDepositionAttorneys == default)
							{
								ReplaceFirstTextOccurrenceOnDocument(documentBody, nameDepositionTemplate, (textInfo.ToTitleCase(attorney.FullName.ToLower()) ?? string.Empty));
								ReplaceFirstTextOccurrenceOnDocument(documentBody, tittleDepositionTemplate, (attorney.AdditionalInfo?.Title ?? string.Empty));
								ReplaceFirstTextOccurrenceOnDocument(documentBody, legalFirmDepositionTemplate, (attorney.AdditionalInfo?.LegalFirm ?? string.Empty));
								ReplaceFirstTextOccurrenceOnDocument(documentBody, addressDepositionTemplate, (attorney.AdditionalInfo?.Address ?? string.Empty));
							}
							else
							{
								Paragraph paragraph = newLineDepositionTemplate.Item1;
								paragraph.InsertAfterSelf(GetNewParagrahFromTemplate(paragraph, (attorney.AdditionalInfo?.Address ?? string.Empty)));
								paragraph.InsertAfterSelf(GetNewParagrahFromTemplate(paragraph, (attorney.AdditionalInfo?.LegalFirm ?? string.Empty)));
								paragraph.InsertAfterSelf(GetNewParagrahFromTemplate(paragraph, $"{(textInfo.ToTitleCase(attorney.FullName.ToLower()) ?? string.Empty)}, {(attorney.AdditionalInfo?.Title ?? string.Empty)}"));
								paragraph.InsertAfterSelf(GetNewParagrahFromTemplate(paragraph, string.Empty));
							}
							amountDepositionAttorneys++;
						}
						break;
				}
			}
			else
			{
				// Probable cause hearing fields
				ReplaceFirstTextOccurrenceOnDocument(documentBody, nameTemplate, string.Empty);
				ReplaceFirstTextOccurrenceOnDocument(documentBody, barTemplate, string.Empty);
				ReplaceFirstTextOccurrenceOnDocument(documentBody, tittleTemplate, string.Empty);
				ReplaceFirstTextOccurrenceOnDocument(documentBody, addressTemplate, string.Empty);
				ReplaceFirstTextOccurrenceOnDocument(documentBody, phoneTemplate, string.Empty);

				// Deposition fields
				ReplaceFirstTextOccurrenceOnDocument(documentBody, nameDepositionTemplate, string.Empty);
				ReplaceFirstTextOccurrenceOnDocument(documentBody, tittleDepositionTemplate, string.Empty);
				ReplaceFirstTextOccurrenceOnDocument(documentBody, legalFirmDepositionTemplate, string.Empty);
				ReplaceFirstTextOccurrenceOnDocument(documentBody, addressDepositionTemplate, string.Empty);
			}
		}

		private void ReplaceFirstTextOccurrenceOnDocument(Body documentBody, string oldText, string newText)
		{
			var text = GetFieldText(documentBody, oldText);
			if (text != null)
			{
				text.Text = newText;
			}
		}

		private void ReplaceAllTextOccurrencesOnDocument(Body documentBody, string oldText, string newText)
		{
			var paragraphs = documentBody.Elements<Paragraph>();

			foreach (var par in paragraphs)
			{
				Text prevText = new Text();
				foreach (var run in par.Elements<Run>())
				{
					foreach (var text in run.Elements<Text>())
					{
						if (text.Text == oldText)
						{
							text.Text = newText;
						}
						prevText = text;
					}
				}
			}
		}

		private Text GetFieldText(Body documentBody, string textString)
		{
			var paragraphs = documentBody.Elements<Paragraph>();

			foreach (var par in paragraphs)
			{
				foreach (var run in par.Elements<Run>())
				{
					foreach (var text in run.Elements<Text>())
					{
						if (text.Text.Contains(textString))
						{
							return text;
						}
					}
				}
			}
			return null;
		}

		private Tuple<Paragraph, Run> GetRunText(Body documentBody, string textString)
		{
			var paragraphs = documentBody.Elements<Paragraph>();

			foreach (var par in paragraphs)
			{
				foreach (var run in par.Elements<Run>())
				{
					foreach (var text in run.Elements<Text>())
					{
						if (text.Text.Contains(textString))
						{
							return new Tuple<Paragraph, Run>(par, run);
						}
					}
				}
			}
			return null;
		}

		private OpenXmlElement GetNewParagrahFromTemplate(Paragraph template, string paragraphText)
		{
			var newParagraph = template.CloneNode(true);
			var lastRun = newParagraph.Elements<Run>().Last();
			var lastText = lastRun.Elements<Text>().Last();
			lastText.Text = paragraphText;
			return newParagraph;
		}

		private void ReplaceAllTextOccurrencesOnDocumentHeader(IEnumerable<HeaderPart> documentHeaders, string oldText, string newText)
		{
			foreach (HeaderPart documentHeader in documentHeaders)
			{
				foreach (Paragraph currentParagraph in documentHeader.RootElement.Descendants<Paragraph>())
				{
					Text prevText = new Text();

					foreach (Run run in currentParagraph.Elements<Run>())
					{
						foreach (Text text in run.Elements<Text>())
						{
							if (text.Text.Contains(oldText))
							{
								text.Text = text.Text.Replace(oldText, newText);
							}
							prevText = text;
						}
					}
				}
			}
		}

		private void FillTableCell(Table introTable, string oldText, string newText)
		{
			foreach (TableRow row in introTable.Elements<TableRow>())
			{
				foreach (TableCell cell in row.Elements<TableCell>())
				{
					foreach (Paragraph paragraph in cell.Elements<Paragraph>())
					{
						foreach (Run run in paragraph.Elements<Run>())
						{
							foreach (Text text in run.Elements<Text>())
							{
								if (text.Text.Equals(oldText))
								{
									text.Text = newText;
								}
							}
						}
					}
				}

			}
		}

		private void FillTranscriptionParagraphs(Body documentBody, IEnumerable<TranscriptDialog> formatedDialog, TranscriptTypeEnum transcriptType)
		{
			Tuple<Paragraph, Run> transcriptionObject = GetRunText(documentBody, "TTRANSCRIPT");
			Paragraph transcriptionParagraph = transcriptionObject.Item1;
			
			foreach (TranscriptDialog segment in formatedDialog)
			{
				string transcriptionText = segment.Transcription;

				if (!segment.ExaminationType.Equals(TranscriptExaminationEnum.None))
				{
					var transcriptionSplited = transcriptionText.Split(':');
					if (segment.ExaminationType == TranscriptExaminationEnum.Direct || segment.ExaminationType == TranscriptExaminationEnum.Cross)
					{
						string sectionTitleText = segment.ExaminationType == Enums.TranscriptExaminationEnum.Direct ? "DIRECT EXAMINATION" : "CROSS EXAMINATION";
						var sectionTitle = GetParagraphForExaminationTitle(sectionTitleText);

						if (segment.Equals(formatedDialog.ElementAt(0)))
						{
							ReplaceFirstTextOccurrenceOnDocument(documentBody, "TTRANSCRIPT", sectionTitleText);
							transcriptionParagraph.ParagraphProperties.Remove();
							transcriptionParagraph.ParagraphProperties = new ParagraphProperties();
							transcriptionParagraph.ParagraphProperties.Justification = (Justification)sectionTitle.ParagraphProperties.Justification.CloneNode(true);
						}
						else
						{
							transcriptionParagraph.InsertAfterSelf(sectionTitle);
							transcriptionParagraph = sectionTitle;
						}

						var sectionSubTitle = GetParagraphForExamination($"BY {transcriptionSplited[0]}:");
						transcriptionParagraph.InsertAfterSelf(sectionSubTitle);
						transcriptionParagraph = sectionSubTitle;
					}

					switch (segment.ExaminationTag)
					{
						case TranscriptExaminationTagEnum.None:
							break;
						case TranscriptExaminationTagEnum.Question:
							transcriptionSplited[0] = "Q";
							break;
						case TranscriptExaminationTagEnum.Answer:
							transcriptionSplited[0] = "A";
							break;
						default:
							throw new ApplicationException(ERROR_TRANSCRIPT_TAG_NOT_FOUND);
					}

					transcriptionSplited[1] = string.Concat("  ", transcriptionSplited[1]);
					transcriptionText = String.Join(":", transcriptionSplited);


					Paragraph paragraph;
					switch (transcriptType)
					{
						case TranscriptTypeEnum.ProbableCauseHearing:
							bool isExaminationSpeakerTag = segment.ExaminationTag.Equals(TranscriptExaminationTagEnum.None);
							paragraph = isExaminationSpeakerTag ? GetParagraphFromTranscriptionSegment(transcriptionText) : GetParagraphForExamination(transcriptionText);
							break;
						case TranscriptTypeEnum.Deposition:
							bool isQuestionTag = segment.ExaminationTag.Equals(TranscriptExaminationTagEnum.Question);
							paragraph = GetParagraphForDepositionSegment(transcriptionText, isQuestionTag);
							break;
						default:
							throw new ApplicationException(ERROR_TRANSCRIPT_TYPE_NOT_FOUND);

					}

					transcriptionParagraph.InsertAfterSelf(paragraph);

					transcriptionParagraph = paragraph;
				}
				else
				{
					Paragraph paragraph = null;
					switch (transcriptType)
					{
						case TranscriptTypeEnum.ProbableCauseHearing:
							paragraph = GetParagraphFromTranscriptionSegment(transcriptionText);
							break;
						case TranscriptTypeEnum.Deposition:
							paragraph = GetParagraphForDepositionSegment(transcriptionText);
							break;
						default:
							throw new ApplicationException(ERROR_TRANSCRIPT_TYPE_NOT_FOUND);

					}

					if (segment.Equals(formatedDialog.ElementAt(0)))
					{
						ReplaceFirstTextOccurrenceOnDocument(documentBody, "TTRANSCRIPT", transcriptionText);
						if (transcriptType.Equals(TranscriptTypeEnum.Deposition))
						{
							transcriptionParagraph.ParagraphProperties.Remove();
							transcriptionParagraph.ParagraphProperties = new ParagraphProperties();
							transcriptionParagraph.ParagraphProperties.Append(new SpacingBetweenLines() { After = "0", Before = "0", Line = "480", LineRule = LineSpacingRuleValues.Auto });
							transcriptionParagraph.Elements<Run>().FirstOrDefault().Append(new TabChar());
							transcriptionParagraph.Elements<Run>().FirstOrDefault().Append(new TabChar());
							transcriptionParagraph.Elements<Run>().FirstOrDefault().Append(new TabChar());
						}
					}
					else
					{
						transcriptionParagraph.InsertAfterSelf(paragraph);
						transcriptionParagraph = paragraph;
					}
				}
			}
		}

		private string GetDeponentVersusField(IEnumerable<TranscriptPerson> persons)
		{
			string vsText = string.Concat(
					persons.Where(x => x.Type == Enums.PersonType.Plaintiff).FirstOrDefault()?.FullName?.ToUpper(),
					" vs ",
					persons.Where(x => x.Type == Enums.PersonType.Defendant).FirstOrDefault()?.FullName?.ToUpper());

			return vsText;
		}

		private string GetDeponentFullName(IEnumerable<TranscriptPerson> persons)
		{
			string deponentFullName = string.Concat(
				persons.Where(x => x.Type == Enums.PersonType.Deponent).FirstOrDefault()?.FirstName?.ToUpper(), " ",
				persons.Where(x => x.Type == Enums.PersonType.Deponent).FirstOrDefault()?.MiddleName?.ToUpper(), " ",
				persons.Where(x => x.Type == Enums.PersonType.Deponent).FirstOrDefault()?.LastName?.ToUpper()
				);

			return Regex.Replace(deponentFullName, " {2,}", " ");
		}
	}
}
