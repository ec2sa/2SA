using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace eArchiver.Models.Repositories.Shared
{
    public class ClientsRepository
    {

        EArchiverDataContext _context = new EArchiverDataContext();
        
        public IQueryable<Client> GetAllClients()
        {
            return _context.Clients.OrderBy(c => c.ClientName);
        }

        public List<Client> GetUserClients(Guid userGUID)
        {
            List<Client> clients = new List<Client>();
            foreach (pGetCIDResult cid in _context.GetCID(userGUID))
            {
                Client client = _context.Clients.Single(c=>c.ClientID == cid.ClientID);
                if(client != null)
                    clients.Add(client);
            }

            return clients;
        }

        public List<int> GetCID(Guid userGUID)
        {
            List<int> result = new List<int>();
            foreach (pGetCIDResult cid in _context.GetCID(userGUID))
            {
                result.Add(cid.ClientID);
            }

            return result;
        }

        public string GetClientName(int clientID)
        {
            return _context.Clients.Single(c => c.ClientID == clientID).ClientName;
        }

        public void CreateClient(Client newClient)
        {
            _context.CreateClient(newClient.ClientName, newClient.ClientDescription, newClient.ClientPrefix, newClient.IsActive);
            
        }

        public Client GetClient(int clientID)
        {
            return _context.Clients.SingleOrDefault(c => c.ClientID == clientID);
        }

        public bool AssignUserToClient(Guid userID, int clientID)
        {
            try
            {
                UsersInClient assignment = new UsersInClient() { ClientID = clientID, UserID = userID };
                _context.UsersInClients.InsertOnSubmit(assignment);
                SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                return false;
            }
        }

        public void SubmitChanges()
        {
            _context.SubmitChanges();
        }

    }
}
