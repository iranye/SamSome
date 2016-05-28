select binnumber, caseid, mediaID, media, tblmedia.notes from tblmedia 
join tlkpmediatype on 
tlkpmediatype.mediatypeid = tblmedia.mediatypeid
where tblmedia.caseid in (2152)and tblmedia.mediatypeid not in (7, 10, 12) order by caseid