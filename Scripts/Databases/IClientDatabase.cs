using System.Collections.Generic;

namespace Databases
{
    public interface IClientDatabase{
        List<object[]> SelectAll(DAOBase dao);
    }
}
