using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eArchiver.Models.Entities;
using eArchiver.Models.SearchCriterias;
using System.Data.Linq;
using eArchiver.Controllers;
using System.Text;
using eArchiver.Helpers;
using eArchiver.Models.ViewModels.Documents;

namespace eArchiver.Models.Repositories.Documents
{
    public class DocumentRepository
    {
        EArchiverDataContext _context = new EArchiverDataContext();

        public Guid CreateDocument(Guid userName)
        {
            Guid? id = null;
            _context.CreateDocument(userName, AppContext.GetCID(), ref id);
            return id.Value;
        }

        public void CreateInfoTypeOne(InfoTypeOne infoTypeOne)
        {
            _context.InfoTypeOnes.InsertOnSubmit(infoTypeOne);
        }

        public void CreateInfoTypeTwo(InfoTypeTwo infoTypeTwo)
        {
            _context.InfoTypeTwos.InsertOnSubmit(infoTypeTwo);
        }

        public void SetDocumentScans(List<Guid> scansIDs, Guid documentID)
        {
            foreach (Guid scanID in scansIDs)
            {
                Scan scan = _context.Scans.Single(s => s.ScanID == scanID);
                scan.DocumentID = documentID;
            }
        }

        public void SetDocumentScan(Guid scanID, Guid documentID)
        {
            if (scanID != null && documentID != null)
            {
                Scan scan = _context.Scans.Single(s => s.ScanID == scanID);
                scan.DocumentID = documentID;
            }
        }

        public void RemoveDocumentScans(List<Guid> scansIDs)
        {
            foreach (Guid scanId in scansIDs)
            {
                Scan scan = _context.Scans.Single(s => s.ScanID == scanId);
                
                if (scan.OriginalScanID != null)
                {
                    Guid originalID = scan.OriginalScanID.Value;

                    List<Scan> scanVersions = _context.Scans.Where(s => s.OriginalScanID == originalID).ToList();

                    scanVersions.ForEach(delegate(Scan scanVersion)
                    {
                        scanVersion.OriginalScanID = null;
                        scanVersion.DocumentID = null;
                    });

                    Scan originalScan = _context.Scans.Single(s => s.ScanID == originalID);
                    originalScan.DocumentID = null;
                    scan.OriginalScanID = null;
                }
                else
                {
                    scan.DocumentID = null;
                }
            }

            #region Usuwanie całkowite wersji i zostawienie oryginalu
            //foreach (Guid scanID in scansIDs)
            //{
            //    Scan scan = _context.Scans.Single(s => s.ScanID == scanID);
                
            //    if (scan.OriginalScanID != null)
            //    {
            //        Guid originalID = scan.OriginalScanID.Value;
                    
            //        List<Scan> scanVersions = _context.Scans.Where(s => s.OriginalScanID == originalID).ToList();
                    
            //        //najpierw usuwa preview
            //        scanVersions.ForEach(delegate(Scan scanVersion){
            //            ScanPreview preview = _context.ScanPreviews.SingleOrDefault(p=>p.ScanID == scanVersion.ScanID);
            //            if(preview != null)
            //                _context.ScanPreviews.DeleteOnSubmit(preview);
            //        });
                    
            //        _context.Scans.DeleteAllOnSubmit<Scan>(scanVersions);
            //        Scan originalScan = _context.Scans.Single(s => s.ScanID == originalID);
            //        originalScan.DocumentID = null;
            //    }
            //    else
            //    {
            //        scan.DocumentID = null;
            //    }
            //}
            #endregion
        }

        public Document GetDocument(Guid documentId)
        {
            try
            {
                return _context.Documents.Single(d => d.DocumentID == documentId);
            }
            catch (InvalidOperationException) { return null; }
        }

        public InfoTypeOne GetInfoTypeOne(int typeOneId)
        {
            try
            {
                return _context.InfoTypeOnes.Single(d => d.TypeOneID == typeOneId);
            }
            catch (InvalidOperationException) { return null; }
        }

        public InfoTypeOne GetInfoTypeOne(Guid documentID)
        {
            try
            {
                return _context.InfoTypeOnes.FirstOrDefault(d => d.DocumentID == documentID);
            }
            catch  { return null; }
        }

        public InfoTypeTwo GetInfoTypeTwo(int typeTwoId)
        {
            try
            {
                return _context.InfoTypeTwos.FirstOrDefault(d => d.TypeTwoId == typeTwoId);
            }
            catch (InvalidOperationException) { return null; }
        }

        public InfoTypeTwo GetInfoTypeTwo(Guid documentID)
        {
            try
            {
                return _context.InfoTypeTwos.FirstOrDefault(d => d.DocumentID == documentID);
            }
            catch (InvalidOperationException) { return null; }
        }

        public IQueryable<Guid> GetScanIDs(Guid documentID)
        {
            ISingleResult<pGetScansFromDocumentResult> result = _context.GetScansFromDocument(documentID);
            return result.Select(s => s.scanID).AsQueryable();
            //IQueryable<Scan> scans = _context.Scans.Where(s => s.DocumentID == documentID);
            //return scans.Select(s=>s.ScanID);

        }

        public bool HasVersion(Guid scanID)
        {
            try
            {
                Scan scan = _context.Scans.Where(s => s.ScanID == scanID).Single();
                if(scan.OriginalScanID != null)
                    return true;
                else
                {
                    IQueryable<Scan> vers = _context.Scans.Where(s => s.OriginalScanID == scanID);
                    if(vers.Count() > 0)
                        return true;
                }

                return false;
            }
            catch {
                return false;
            }
        }

        public List<DocumentDetails> SearchDocuments(DocumentsSearchCriteria searchCriteria)
        {
            List<DocumentDetails> result = new List<DocumentDetails>();

            IQueryable<DocumentInfo> documents = _context.DocumentInfos.Where(s => s.ClientID == AppContext.GetCID());

            if(searchCriteria.DocumentNumber != null)
            {
                documents = documents.Where(d => d.DocumentNumber.StartsWith(searchCriteria.DocumentNumber));
            }

            if(searchCriteria.CaseNumber != null)
            {
                documents = documents.Where(d => d.CaseNumber.StartsWith(searchCriteria.CaseNumber));
            }

            if (searchCriteria.Date.HasValue && searchCriteria.Date != DateTime.MinValue)
                documents = documents.Where(d => d.Date.Value.Date >= searchCriteria.Date.Value.Date);

            if (searchCriteria.DateTo.HasValue && searchCriteria.DateTo != DateTime.MinValue)
                documents = documents.Where(d => d.Date.Value.Date <= searchCriteria.DateTo.Value.Date);
            
            if (searchCriteria.CategoryID.HasValue)
                documents = documents.Where(d => d.CategoryID == searchCriteria.CategoryID.Value);
            
            if (searchCriteria.TypeID.HasValue)
                documents = documents.Where(d => d.TypeID == searchCriteria.TypeID.Value);

            if (searchCriteria.Type2ID.HasValue)
                documents = documents.Where(d => d.Type2ID == searchCriteria.Type2ID.Value);
            
            if (!string.IsNullOrEmpty(searchCriteria.Company) )
            {
                var ds = _context.Senders.Where(s => s.Company.ToLower().Contains(searchCriteria.Company.ToLower())).Select(s=>s.SenderID).ToList();

                var ds2 = _context.DocumentSenders.Where(d =>ds.Contains(d.SenderID)).Select(d => d.DocumentID).ToList();
                documents = documents = documents.Where(d => ds2.Contains(d.DocumentID));
            }

            if (searchCriteria.SenderID.HasValue)
            {
                var ds = _context.DocumentSenders.Where(d => d.SenderID == searchCriteria.SenderID).Select(d => d.DocumentID).ToList();
                documents=documents.Where(d=> ds.Contains(d.DocumentID));
            }

            if (!string.IsNullOrEmpty(searchCriteria.Description))
                documents = documents.Where(d => d.Description.Contains(searchCriteria.Description));

            if (searchCriteria.CreateDate.HasValue && searchCriteria.CreateDate != DateTime.MinValue)
                documents = documents.Where(d => d.CreateDate.Date>=searchCriteria.CreateDate.Value.Date);

            if (searchCriteria.CreateDateTo.HasValue && searchCriteria.CreateDateTo != DateTime.MinValue)
                documents = documents.Where(d => d.CreateDate.Date <= searchCriteria.CreateDateTo.Value.Date);

            #region serch by tags
            
            if (!string.IsNullOrEmpty(searchCriteria.Tags))
                documents=applySearchByTags(documents, searchCriteria.Tags);
            
            #endregion

            #region Search In OCR Content
            Dictionary<Guid, Dictionary<Guid, string>> contentInfos = new Dictionary<Guid, Dictionary<Guid, string>>();
            if (!string.IsNullOrEmpty(searchCriteria.Text))
            {
                List<Scan> scans = new List<Scan>();
                foreach (var document in documents)
                {
                    scans.AddRange(GetDocument(document.DocumentID).Scans.ToList());
                }

                IQueryable<OcrContent> ocrContentList = _context.OcrContents
                    .Where(c => scans.Select(s => s.ScanID).Contains(c.ScanID));
                    
                if(!searchCriteria.SearchOCR)
                    ocrContentList = ocrContentList.Where(c => c.PageNumber == 0);

                ocrContentList = ocrContentList.Where(c => c.TextContent.Contains(searchCriteria.Text));

                foreach (var ocrContent in ocrContentList)
                {
                    //jesli skan jest w jakims dokumencie
                    if(ocrContent.Scan.DocumentID.HasValue)
                    {
                        //dodajemy informacje o skanach tego dokumentu
                        if (!contentInfos.ContainsKey(ocrContent.Scan.DocumentID.Value))
                        {
                            Dictionary<Guid, string> dict = new Dictionary<Guid, string>();
                            dict.Add(ocrContent.ScanID, string.Format("{0} ({1})", ocrContent.Scan.FileName, ocrContent.PageNumber));

                            contentInfos.Add(ocrContent.Scan.DocumentID.Value, dict);
                        }
                        //a jeśli już wcześniej dodaliśmy ten dokument
                        else
                        {
                            //sprawdzamy czy dodaliśmy cos o tym skanie
                            if (contentInfos[ocrContent.Scan.DocumentID.Value].ContainsKey(ocrContent.ScanID))
                            {
                                contentInfos[ocrContent.Scan.DocumentID.Value][ocrContent.ScanID] =
                                    contentInfos[ocrContent.Scan.DocumentID.Value][ocrContent.ScanID].Replace(")", string.Concat(", ", ocrContent.PageNumber, ")"));
                            }
                            //dodajemy informacje o kolejnym skanie w tym dokumencie
                            else
                            {
                                contentInfos[ocrContent.Scan.DocumentID.Value].Add(ocrContent.ScanID, string.Format("{0}({1})", ocrContent.Scan.FileName, ocrContent.PageNumber));
                            }
                        }
                    }
                }

                List<Guid> scansWithTextIDs = ocrContentList.Select(c=>c.ScanID).Distinct().ToList();
                
                List<Guid?> documentIDs = _context.Scans
                    .Where(s => scansWithTextIDs.Contains(s.ScanID))
                    .Select(s => s.DocumentID).Distinct().ToList();

                documents = documents.Where(d => documentIDs.Contains(d.DocumentID));
            }
            #endregion

            foreach(DocumentInfo d in documents.ToList())
            {
                DocumentDetails details = GetDocumentDetails(d.DocumentID);
                details.DocumentSenders = d.Sender;
                if (contentInfos.ContainsKey(d.DocumentID))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var scansDict in contentInfos[d.DocumentID])
                        sb.Append(scansDict.Value).Append(", ");
                    string scanContentInfo = sb.ToString().Substring(0, sb.ToString().Length-2);
                    details.ScanContentInfo = scanContentInfo.Replace("(0)","");
                }
                result.Add(details);
            }
            
            return result;
        }
  
        public DocumentDetails GetDocumentDetails(Guid documentID)
        {
            return new DocumentDetails()
            {
                Document = GetDocument(documentID),
                InfoTypeOne = GetInfoTypeOne(documentID),
                InfoTypeTwo = GetInfoTypeTwo(documentID),
                ScanIDs = GetScanIDs(documentID).ToList(),
                Senders = GetSenders(documentID)
                

            };
        }

        public List<Sender> GetSenders(Guid documentID)
        {
            var senderIDs = _context.DocumentSenders.Where(ds => ds.DocumentID == documentID).Select(s=>s.SenderID).ToList();
            return _context.Senders.Where(s => senderIDs.Contains(s.SenderID)).ToList();
        }

        public void AddScanVersion(Guid originalScanID, string fileName, string mimeType, Binary fileContent, Binary previewContent, string previewMimeType, Binary zoomContent, int clientID)
        {
            _context.AddScanVersion(originalScanID, fileName, mimeType, fileContent, previewContent, previewMimeType,zoomContent, clientID);
        }

        /// <summary>
        /// Adds new version after submit.
        /// </summary>
        /// <param name="originalScanId"></param>
        /// <param name="versionScanId"></param>
        public void AddScanVersion(Guid originalScanId, Guid versionScanId, Guid documentId)
        {
            Scan versionScan = _context.Scans.Where(s => s.ScanID == versionScanId).Single();
            Scan originalScan = _context.Scans.Where(s => s.ScanID == originalScanId).Single();
            
            if (versionScan != null)
            {
                if (originalScan.OriginalScanID == null)
                { versionScan.OriginalScanID = originalScanId; }
                else
                { versionScan.OriginalScanID = originalScan.OriginalScanID; }

                versionScan.DocumentID = documentId;
            }
        }
        
        public Guid? GetScansDocumentID(Guid scanID)
        {
            Guid? docID = _context.Scans.Where(s => s.ScanID == scanID).Select(s => s.DocumentID).SingleOrDefault();
            return docID;
        }

        public void SubmitChanges()
        {
            _context.SubmitChanges();
        }

        public IQueryable<DocumentHistory> GetDocumentHistory(Guid documentID)
        {
            var clientCheck = _context.Documents.FirstOrDefault(d => AppContext.GetCIDs().Contains(d.ClientID));
            if (clientCheck == null)
                return null;
            return _context.DocumentHistories.Where(d=>d.DocumentID == documentID);
        }

        private IQueryable<DocumentInfo> applySearchByTags(IQueryable<DocumentInfo> documents, string tags)
        {
            //"[11],[24],[12]"  - or
            //"[11]+[24]+[12]"  - and
            tags = tags.Replace(" ", "");
            char separator=' ';
            if (tags.Contains('+'))
                separator = '+';
            else if (tags.Contains(','))
                separator = ',';
            else
            {
                
                return documents.Where(d=>d.Tags.Contains(tags));
            }

            string[] splittedTags = tags.Split(separator);

            if (separator == ',')
            {
                var predicate = PredicateBuilder.False<DocumentInfo>();
                foreach (string tag in splittedTags)
                {
                    string temp = tag;
                    predicate = predicate.Or(p => p.Tags.Contains(temp));
                }
                return documents.Where(predicate);
            }
            else
            {
                var predicate = PredicateBuilder.True<DocumentInfo>();
                foreach (string tag in splittedTags)
                {
                    string temp = tag;
                    predicate = predicate.And(p => p.Tags.Contains(temp));
                }
                return documents.Where(predicate);
            }
            
            ;

        }

        public void AddSender(Guid documentID, int senderID)
        {
            _context.DocumentSenders.InsertOnSubmit(new DocumentSender() {DocumentID=documentID,SenderID=senderID });
            _context.SubmitChanges();
        }

        public void RemoveSenders(Guid documentID)
        {
            var toDelete = _context.DocumentSenders.Where(ds => ds.DocumentID == documentID);
            _context.DocumentSenders.DeleteAllOnSubmit(toDelete);
            _context.SubmitChanges();
        }

        public DocumentSearchResult SearchDocuements(DocumentsSearchCriteria searchCriteria, int page, int itemsOnPage)
        {
            DocumentSearchResult searchResult = new DocumentSearchResult()
            {
                
            };
            return new DocumentSearchResult();
        }
    }
}
