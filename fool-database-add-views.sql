
--Use this approach for SQLS 2017 and later
create view dbo.vw_subscriptions as
select u.UserId, u.FirstName, u.LastName, u.Email, STRING_AGG(i.instrumentId, ',') Instruments
from [User] u join UserWatchedInstrument uwi on u.UserId = uwi.UserId
join Instrument i on uwi.InstrumentId = i.InstrumentId
join UserEmailSubscription ues on u.UserId = ues.UserId
where ues.EmailSubscriptionId = 12 and i.InstrumentId in (1,2,3,4,5,6)
group by u.UserId, u.FirstName, u.LastName, u.Email
 
--Use this approach for SQLS 2016 and earlier
/*
create view dbo.vw_subscriptions as
select distinct u.UserId, u.FirstName, u.LastName, u.Email,
stuff((select ',' + convert(nvarchar(3), i.InstrumentId) 
	from [User] u2
	join UserWatchedInstrument uwi on u.UserId = uwi.UserId
	join Instrument i on uwi.InstrumentId = i.InstrumentId
	join UserEmailSubscription ues on u.UserId = ues.UserId
	where u2.UserId = u.UserId and i.InstrumentId is not null
	and ues.EmailSubscriptionId = 12 and i.InstrumentId in (1,2,3,4,5,6)
	for xml path('')),1,1,'') Instruments
from [User] u 
	join UserWatchedInstrument uwi on u.UserId = uwi.UserId
	join Instrument i on uwi.InstrumentId = i.InstrumentId
	join UserEmailSubscription ues on u.UserId = ues.UserId
	where u.UserId = u.UserId and i.InstrumentId is not null
	and ues.EmailSubscriptionId = 12 and i.InstrumentId in (1,2,3,4,5,6)
*/