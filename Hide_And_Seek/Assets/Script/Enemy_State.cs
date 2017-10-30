using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface Enemy_State
{
    float speed { get; set; }

    void do_action();
    void stop_action();
}