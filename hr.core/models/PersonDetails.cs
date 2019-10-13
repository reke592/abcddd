namespace hr.core.models
{
  public class PersonDetails : Entity
  {
    public virtual string FirstName { get; protected set; }
    public virtual string MiddleName { get; protected set; }
    public virtual string LastName { get; protected set; }
    public virtual string ExtName { get; protected set; }
    public virtual string Gender { get; protected set; }

    public virtual void Rename(string firstname, string middlename, string lastname, string ext) {
      FirstName = firstname;
      MiddleName = middlename;
      LastName = lastname;
      ExtName = ext;
    }
  }
}