using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Hub de notificaciones en tiempo real. La entrega se hace por usuario con
/// <c>Clients.User(usuarioId)</c>; SignalR resuelve el destino a partir del claim
/// NameIdentifier del JWT (IUserIdProvider por defecto). El servidor empuja el
/// evento "nueva"; el cliente solo necesita suscribirse.
/// Se mantiene en el namespace global (como estaba) para no romper Program.cs.
/// </summary>
public class NotificacionesHub : Hub
{
}
