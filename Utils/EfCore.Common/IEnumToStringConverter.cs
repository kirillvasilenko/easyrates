namespace EfCore.Common
{
    public interface IEnumToStringConverter<T>
        where T: struct
    {
        string ToString(T enumValue);

        T ToEnum(string strValue);
    }
}