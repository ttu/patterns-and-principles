/*
 * It is not reccomended that strategy takes the context as a parameter and changes it's state
 * If context changes then strategy will need also updates
 * 
 * Check input handling as a strategy from Lego Boost AI
 * https://github.com/ttu/lego-boost-ai/blob/0942048b8f724b693893a3362cd45fbca710c29b/input-modes.js
 * 
 * Set original strategy:
 * https://github.com/ttu/lego-boost-ai/blob/0942048b8f724b693893a3362cd45fbca710c29b/index.js#L4
 * https://github.com/ttu/lego-boost-ai/blob/0942048b8f724b693893a3362cd45fbca710c29b/index.js#L33
 * 
 * Set stategy in use (runtime):
 * https://github.com/ttu/lego-boost-ai/blob/0942048b8f724b693893a3362cd45fbca710c29b/index.js#L61
 * 
 * Handle input with selected strategy:
 * https://github.com/ttu/lego-boost-ai/blob/0942048b8f724b693893a3362cd45fbca710c29b/index.js#L54
*/