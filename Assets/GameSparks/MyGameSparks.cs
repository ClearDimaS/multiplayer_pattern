#pragma warning disable 612,618
#pragma warning disable 0114
#pragma warning disable 0108

using System;
using System.Collections.Generic;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!

namespace GameSparks.Api.Requests{
		public class LogEventRequest_DisplayNameRequest : GSTypedRequest<LogEventRequest_DisplayNameRequest, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_DisplayNameRequest() : base("LogEventRequest"){
			request.AddString("eventKey", "DisplayNameRequest");
		}
	}
	
	public class LogChallengeEventRequest_DisplayNameRequest : GSTypedRequest<LogChallengeEventRequest_DisplayNameRequest, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_DisplayNameRequest() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "DisplayNameRequest");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_DisplayNameRequest SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_RATING_UPDATE : GSTypedRequest<LogEventRequest_RATING_UPDATE, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_RATING_UPDATE() : base("LogEventRequest"){
			request.AddString("eventKey", "RATING_UPDATE");
		}
		public LogEventRequest_RATING_UPDATE Set_Rating( long value )
		{
			request.AddNumber("Rating", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_RATING_UPDATE : GSTypedRequest<LogChallengeEventRequest_RATING_UPDATE, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_RATING_UPDATE() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "RATING_UPDATE");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_RATING_UPDATE SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_RATING_UPDATE Set_Rating( long value )
		{
			request.AddNumber("Rating", value);
			return this;
		}			
	}
	
	public class LogEventRequest_upgradeGuestAccount : GSTypedRequest<LogEventRequest_upgradeGuestAccount, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_upgradeGuestAccount() : base("LogEventRequest"){
			request.AddString("eventKey", "upgradeGuestAccount");
		}
		
		public LogEventRequest_upgradeGuestAccount Set_username( string value )
		{
			request.AddString("username", value);
			return this;
		}
		
		public LogEventRequest_upgradeGuestAccount Set_displayName( string value )
		{
			request.AddString("displayName", value);
			return this;
		}
		
		public LogEventRequest_upgradeGuestAccount Set_password( string value )
		{
			request.AddString("password", value);
			return this;
		}
		
		public LogEventRequest_upgradeGuestAccount Set_email( string value )
		{
			request.AddString("email", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_upgradeGuestAccount : GSTypedRequest<LogChallengeEventRequest_upgradeGuestAccount, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_upgradeGuestAccount() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "upgradeGuestAccount");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_upgradeGuestAccount SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_upgradeGuestAccount Set_username( string value )
		{
			request.AddString("username", value);
			return this;
		}
		public LogChallengeEventRequest_upgradeGuestAccount Set_displayName( string value )
		{
			request.AddString("displayName", value);
			return this;
		}
		public LogChallengeEventRequest_upgradeGuestAccount Set_password( string value )
		{
			request.AddString("password", value);
			return this;
		}
		public LogChallengeEventRequest_upgradeGuestAccount Set_email( string value )
		{
			request.AddString("email", value);
			return this;
		}
	}
	
	public class LogEventRequest_WinRequest : GSTypedRequest<LogEventRequest_WinRequest, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_WinRequest() : base("LogEventRequest"){
			request.AddString("eventKey", "WinRequest");
		}
		public LogEventRequest_WinRequest Set_Win( long value )
		{
			request.AddNumber("Win", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_WinRequest : GSTypedRequest<LogChallengeEventRequest_WinRequest, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_WinRequest() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "WinRequest");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_WinRequest SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_WinRequest Set_Win( long value )
		{
			request.AddNumber("Win", value);
			return this;
		}			
	}
	
}
	
	
	
namespace GameSparks.Api.Requests{
	
	public class LeaderboardDataRequest_LeaderboardRating : GSTypedRequest<LeaderboardDataRequest_LeaderboardRating,LeaderboardDataResponse_LeaderboardRating>
	{
		public LeaderboardDataRequest_LeaderboardRating() : base("LeaderboardDataRequest"){
			request.AddString("leaderboardShortCode", "LeaderboardRating");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LeaderboardDataResponse_LeaderboardRating (response);
		}		
		
		/// <summary>
		/// The challenge instance to get the leaderboard data for
		/// </summary>
		public LeaderboardDataRequest_LeaderboardRating SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		/// <summary>
		/// The number of items to return in a page (default=50)
		/// </summary>
		public LeaderboardDataRequest_LeaderboardRating SetEntryCount( long entryCount )
		{
			request.AddNumber("entryCount", entryCount);
			return this;
		}
		/// <summary>
		/// A friend id or an array of friend ids to use instead of the player's social friends
		/// </summary>
		public LeaderboardDataRequest_LeaderboardRating SetFriendIds( List<string> friendIds )
		{
			request.AddStringList("friendIds", friendIds);
			return this;
		}
		/// <summary>
		/// Number of entries to include from head of the list
		/// </summary>
		public LeaderboardDataRequest_LeaderboardRating SetIncludeFirst( long includeFirst )
		{
			request.AddNumber("includeFirst", includeFirst);
			return this;
		}
		/// <summary>
		/// Number of entries to include from tail of the list
		/// </summary>
		public LeaderboardDataRequest_LeaderboardRating SetIncludeLast( long includeLast )
		{
			request.AddNumber("includeLast", includeLast);
			return this;
		}
		
		/// <summary>
		/// The offset into the set of leaderboards returned
		/// </summary>
		public LeaderboardDataRequest_LeaderboardRating SetOffset( long offset )
		{
			request.AddNumber("offset", offset);
			return this;
		}
		/// <summary>
		/// If True returns a leaderboard of the player's social friends
		/// </summary>
		public LeaderboardDataRequest_LeaderboardRating SetSocial( bool social )
		{
			request.AddBoolean("social", social);
			return this;
		}
		/// <summary>
		/// The IDs of the teams you are interested in
		/// </summary>
		public LeaderboardDataRequest_LeaderboardRating SetTeamIds( List<string> teamIds )
		{
			request.AddStringList("teamIds", teamIds);
			return this;
		}
		/// <summary>
		/// The type of team you are interested in
		/// </summary>
		public LeaderboardDataRequest_LeaderboardRating SetTeamTypes( List<string> teamTypes )
		{
			request.AddStringList("teamTypes", teamTypes);
			return this;
		}
		
	}

	public class AroundMeLeaderboardRequest_LeaderboardRating : GSTypedRequest<AroundMeLeaderboardRequest_LeaderboardRating,AroundMeLeaderboardResponse_LeaderboardRating>
	{
		public AroundMeLeaderboardRequest_LeaderboardRating() : base("AroundMeLeaderboardRequest"){
			request.AddString("leaderboardShortCode", "LeaderboardRating");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new AroundMeLeaderboardResponse_LeaderboardRating (response);
		}		
		
		/// <summary>
		/// The number of items to return in a page (default=50)
		/// </summary>
		public AroundMeLeaderboardRequest_LeaderboardRating SetEntryCount( long entryCount )
		{
			request.AddNumber("entryCount", entryCount);
			return this;
		}
		/// <summary>
		/// A friend id or an array of friend ids to use instead of the player's social friends
		/// </summary>
		public AroundMeLeaderboardRequest_LeaderboardRating SetFriendIds( List<string> friendIds )
		{
			request.AddStringList("friendIds", friendIds);
			return this;
		}
		/// <summary>
		/// Number of entries to include from head of the list
		/// </summary>
		public AroundMeLeaderboardRequest_LeaderboardRating SetIncludeFirst( long includeFirst )
		{
			request.AddNumber("includeFirst", includeFirst);
			return this;
		}
		/// <summary>
		/// Number of entries to include from tail of the list
		/// </summary>
		public AroundMeLeaderboardRequest_LeaderboardRating SetIncludeLast( long includeLast )
		{
			request.AddNumber("includeLast", includeLast);
			return this;
		}
		
		/// <summary>
		/// If True returns a leaderboard of the player's social friends
		/// </summary>
		public AroundMeLeaderboardRequest_LeaderboardRating SetSocial( bool social )
		{
			request.AddBoolean("social", social);
			return this;
		}
		/// <summary>
		/// The IDs of the teams you are interested in
		/// </summary>
		public AroundMeLeaderboardRequest_LeaderboardRating SetTeamIds( List<string> teamIds )
		{
			request.AddStringList("teamIds", teamIds);
			return this;
		}
		/// <summary>
		/// The type of team you are interested in
		/// </summary>
		public AroundMeLeaderboardRequest_LeaderboardRating SetTeamTypes( List<string> teamTypes )
		{
			request.AddStringList("teamTypes", teamTypes);
			return this;
		}
	}
}

namespace GameSparks.Api.Responses{
	
	public class _LeaderboardEntry_LeaderboardRating : LeaderboardDataResponse._LeaderboardData{
		public _LeaderboardEntry_LeaderboardRating(GSData data) : base(data){}
		public long? Rating{
			get{return response.GetNumber("Rating");}
		}
	}
	
	public class LeaderboardDataResponse_LeaderboardRating : LeaderboardDataResponse
	{
		public LeaderboardDataResponse_LeaderboardRating(GSData data) : base(data){}
		
		public GSEnumerable<_LeaderboardEntry_LeaderboardRating> Data_LeaderboardRating{
			get{return new GSEnumerable<_LeaderboardEntry_LeaderboardRating>(response.GetObjectList("data"), (data) => { return new _LeaderboardEntry_LeaderboardRating(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_LeaderboardRating> First_LeaderboardRating{
			get{return new GSEnumerable<_LeaderboardEntry_LeaderboardRating>(response.GetObjectList("first"), (data) => { return new _LeaderboardEntry_LeaderboardRating(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_LeaderboardRating> Last_LeaderboardRating{
			get{return new GSEnumerable<_LeaderboardEntry_LeaderboardRating>(response.GetObjectList("last"), (data) => { return new _LeaderboardEntry_LeaderboardRating(data);});}
		}
	}
	
	public class AroundMeLeaderboardResponse_LeaderboardRating : AroundMeLeaderboardResponse
	{
		public AroundMeLeaderboardResponse_LeaderboardRating(GSData data) : base(data){}
		
		public GSEnumerable<_LeaderboardEntry_LeaderboardRating> Data_LeaderboardRating{
			get{return new GSEnumerable<_LeaderboardEntry_LeaderboardRating>(response.GetObjectList("data"), (data) => { return new _LeaderboardEntry_LeaderboardRating(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_LeaderboardRating> First_LeaderboardRating{
			get{return new GSEnumerable<_LeaderboardEntry_LeaderboardRating>(response.GetObjectList("first"), (data) => { return new _LeaderboardEntry_LeaderboardRating(data);});}
		}
		
		public GSEnumerable<_LeaderboardEntry_LeaderboardRating> Last_LeaderboardRating{
			get{return new GSEnumerable<_LeaderboardEntry_LeaderboardRating>(response.GetObjectList("last"), (data) => { return new _LeaderboardEntry_LeaderboardRating(data);});}
		}
	}
}	

namespace GameSparks.Api.Messages {


}
