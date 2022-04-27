import { HttpHeaders } from "@angular/common/http";

export class Constants {

  public static HeadersContentType = new HttpHeaders({
    "Content-Type": "application/json"
  });

  public static readonly TokenInfo = 'Token';

  public static readonly UserId = 'UserId';
}
