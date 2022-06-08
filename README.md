# Producer-Consumer Problem

① Producer-Consumer Problem
- It is a problem that processes belonging to one group (producer process) create a message and send it to another group (consumer process), and it is assumed that a buffer of shared memory is used in the message delivery process. That is, the producer process creates a message, stores it in the shared memory buffer, and notifies the consumer process so that the consumer process has the message in the buffer.

② How to solve the producer-consumer problem
- The solution is sometimes to use a single buffer and a circular buffer. In the case of using a single buffer, the producer process loads a message into the buffer once, waits for the consumer process to consume it, and then writes another message to the buffer. should be loaded To compensate for this disadvantage, circular multi-buffers are used. In this case, message generation and buffer loading of producer processes are continuously possible without the intervention of consumer processes until all N buffers are full. So, we make a simulation program for the case of using a circular multi-buffer.
