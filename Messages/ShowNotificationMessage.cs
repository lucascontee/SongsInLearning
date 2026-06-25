using SongsInLearning.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongsInLearning.Messages;

public record ShowNotificationMessage(string Message, NotificationType Type, int Delay);
